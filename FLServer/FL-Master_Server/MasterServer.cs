using FL_Master_Server.User;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Authentication;
using Shared.Characters;
using Shared.Extensions;
using Shared.Packets;
using Shared.Security;
using Shared.Users;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace FL_Master_Server
{
    class MasterServer
    {
        private EventBasedNetListener listener;
        private NetManager server;

        private bool running = true;

        List<NetworkUser> NetworkUsers = new List<NetworkUser>();

        Dictionary<int, GameServerInfo> GameServers = new Dictionary<int, GameServerInfo>();
        Dictionary<int, List<NetPeer>> playersWaiting = new Dictionary<int, List<NetPeer>>();

        Random random = new Random();

        public void Run()
        {
            Console.WriteLine("Starting Master server..");
            Console.WriteLine("Assigning serverlistener..");
            listener = new EventBasedNetListener();
            Console.WriteLine($"Serverlistener assigned: {listener.ToString()}");


            Console.WriteLine("Assigning NetManager with serverlistener");
            server = new NetManager(listener);
            server.UnconnectedMessagesEnabled = true;
            Console.WriteLine("Attempting to run server");
            try
            {
                server.Start(Constants.Port);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            listener.ConnectionRequestEvent += request =>
            {
                if (server.PeersCount < Constants.MaxConnections)
                    request.AcceptIfKey(Constants.ConnectionKey);
                else
                    request.Reject();
            };

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
            };

            listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            listener.PeerDisconnectedEvent += OnListenerOnPeerDisconnectedEvent;

            listener.NetworkReceiveUnconnectedEvent += ReceiveUnconnectedMessage;

            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:{Constants.Port}");
            
            while (running)
            {
                server.PollEvents();
                Thread.Sleep(200);
            }

            server.Stop();
        }

        private void ReceiveUnconnectedMessage(IPEndPoint remoteendpoint, NetPacketReader reader, UnconnectedMessageType messagetype)
        {
            //Console.WriteLine("test: " + reader.GetString());


            ushort msgid = reader.GetUShort();

            switch (msgid)
            {
                case 1:
                {
                    int serverPort = reader.GetInt();
                    string masterKey = reader.GetString();
                    byte roomType = reader.GetByte();
                    byte maxPlayers = reader.GetByte();
                    string serverName = reader.GetString();

                    GameServers.Add(serverPort, new GameServerInfo(serverName, serverPort, masterKey, roomType, maxPlayers));

                    Console.WriteLine($"GameServer opened on port {serverPort}");

                    foreach (NetPeer player in playersWaiting[serverPort])
                    {
                        //send server info
                        NetDataWriter writer = new NetDataWriter();
                        writer.Put((ushort) 600);
                        writer.Put(serverPort);
                        writer.Put(masterKey);

                        player.Send(writer, DeliveryMethod.ReliableOrdered);
                    }

                    playersWaiting.Remove(serverPort);
                }
                    break;
                case 2:
                {
                    int serverPort = reader.GetInt();

                    GameServers.Remove(serverPort);
                    Console.WriteLine($"GameServer closed on port {serverPort}");
                }
                    break;
                case 3:
                {
                    int serverPort = reader.GetInt();
                    GameServers[serverPort].totalPlayers++;
                }
                    break;
                case 4:
                {
                    int serverPort = reader.GetInt();
                    GameServers[serverPort].totalPlayers--;
                }
                    break;
            }
        }

        private void OnListenerOnNetworkReceiveEvent(NetPeer fromPeer, NetPacketReader dataReader, DeliveryMethod deliveryMethod)
        {
            ushort msgid = dataReader.GetUShort();

            switch (msgid)
            {
                case 423:
                    {
                        string id = dataReader.GetString();
                        string pwd = Security.GetHashString(dataReader.GetString());
                        Console.WriteLine($"Got a conection from UniquePlayer: {id}");
                        Console.WriteLine($"Verifying the user {id}({id.Length}):{pwd}({pwd.Length})");
                        var friends = UserMethods.GetFriendsAsPacket(id);
                        if (!UserAuth.VerifyPassword(id, pwd))
                        {
                            Console.WriteLine("Authetication failed disconnectin the user");
                            fromPeer.Disconnect();
                        }
                        else
                        {
                            NetDataWriter writer = new NetDataWriter();
                            FLServer.Models.User u = UserMethods.GetUserByUsername(id);
                            writer.Put((ushort)2004);
                            writer.PutPacket(UserMethods.GetUserAsProfilePartInfoPacket(id));
                            writer.PutPackets(UserMethods.GetFriendsAsPacket(id));
                            fromPeer.Send(writer, DeliveryMethod.Unreliable);
                        }
                    }
                    break;
                case 424:
                {
                    string id = dataReader.GetString();
                    NetDataWriter writer = new NetDataWriter();
                        //FLServer.Models.User u = UserMethods.GetUserByUsername(id);
                    ProfileAccountInfo pai = UserMethods.GetProfileAccountInfoPacket(id);

                    if(string.IsNullOrEmpty(pai.ErrorMessage))
                    {
                        writer.Put((ushort)2005);
                        writer.PutPacket(pai);
                        fromPeer.Send(writer, DeliveryMethod.Unreliable);
                    }
                    else
                    {
                        writer.Put((ushort) 2006);
                        writer.PutPacket(pai);
                        fromPeer.Send(writer, DeliveryMethod.Unreliable);
                    }
                }
                    break;
                case 425://Send character
                    {
                        string name = dataReader.GetString();
                        NetDataWriter writer = new NetDataWriter();
                        CharacterInformation charinfo = CharacterMethods.GetCharacterAsCharacterInfoPacket(name);
                        writer.Put((ushort)2016);
                        writer.PutPacket(charinfo);
                        fromPeer.Send(writer, DeliveryMethod.Unreliable);
                        break;
                    }
                case 88:
                {
                }
                    break;
                case 470: //setting avatarTODO: safety
                {
                    string name = dataReader.GetString();
                    int id = dataReader.GetInt();
                    UserMethods.SetAvatar(name, id);
                }
                    break;
                case 600:
                {
                    string serverName = dataReader.GetString();
                    byte maxPlayers = dataReader.GetByte();

                    int serverPort = Constants.StartGameServerPort;

                    while (GameServers.ContainsKey(serverPort))
                    {
                        serverPort++;
                    }

                    string masterKey = random.Next(1000000, 9999999).ToString();


                    playersWaiting.Add(serverPort, new List<NetPeer> {fromPeer});

                    StartGameServer(serverName, serverPort, masterKey, 0, maxPlayers);
                }
                    break;

                case 601:
                {
                    NetDataWriter writer = new NetDataWriter();

                    foreach (var gameServer in GameServers)
                    {
                        if (gameServer.Value.open && gameServer.Value.roomType == 0)
                        {
                            writer.Put((ushort) 601);
                            writer.Put(gameServer.Value.serverName);
                            writer.Put(gameServer.Value.totalPlayers);
                            writer.Put(gameServer.Value.maxPlayers);
                            writer.Put("127.0.0.1");
                            writer.Put(gameServer.Value.masterKey);
                            writer.Put(gameServer.Value.port);

                            fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                            writer.Reset();
                        }
                    }
                }
                    break;
            }

            dataReader.Recycle();
        }

        private void StartGameServer(string serverName, int port, string masterKey, byte roomType, byte maxPlayers)
        {
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\GameServer\\FL_Game_Server.dll");
            string pathToFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                "\\GameServer\\FL_Game_Server.dll";
            //Console.WriteLine($"Starting game server from: {pathToFile} with args:\n{pathToFile} {port} {masterKey} {roomType} {serverName} {maxPlayers}");
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"{pathToFile} {port} {masterKey} {roomType} {serverName} {maxPlayers}",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };

                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not start program " + ex);
            }
        }

        private static void OnListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo info)
        {
            Console.WriteLine($"peer disconnected: {peer.EndPoint}");
        }
    }
}