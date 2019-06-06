using FL_Master_Server.Player;
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
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Threading;
using FLServer.Models;
using FL_Master_Server.Player.Content;
using Shared.Packets.UserState;

namespace FL_Master_Server
{
    public class MasterServer
    {
        //singleton
        private static MasterServer instance = null;
        private MasterServer() { }

        public static MasterServer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MasterServer();
                }
                return instance;
            }
        }

        private Validation validation = new Validation();
        private Util util = new Util();

        private EventBasedNetListener listener;
        private NetManager server;

        private bool running = true;

        public List<NetworkUser> NetworkUsers = new List<NetworkUser>();

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
                NetworkUsers.Add(new NetworkUser(peer, null));
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
                        Console.WriteLine($"Verifying user {id}({id.Length}):{pwd}({pwd.Length})peer:{fromPeer}");
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
                            
                            if(u.Rights < 0)
                            {
                                Console.WriteLine($"Disconnected banned user {u.Username}");
                                fromPeer.Disconnect();
                            }

                            NetworkUser me = util.GetNetworkUserFromPeer(fromPeer);
                            if (me != null)
                                me.User = u;
                            else
                                break; //user not found somehow not connected

                            writer.Put((ushort)2004);
                            writer.PutPacketStruct(UserMethods.GetUserAsProfilePartInfoPacket(u));
                            fromPeer.Send(writer, DeliveryMethod.ReliableOrdered); 
                        }
                    }
                    break;
                case 424:
                {
                    string id = dataReader.GetString();
                    NetDataWriter writer = new NetDataWriter();
                    ProfileAccountInfo pai = UserMethods.GetProfileAccountInfoPacket(id);

                    if(string.IsNullOrEmpty(pai.ErrorMessage))
                    {
                        writer.Put((ushort)2005);
                        writer.PutPacketStruct(pai);
                        fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                    }
                    else
                    {
                        writer.Put((ushort) 2006);
                        writer.PutPacketStruct(pai);
                        fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                    }
                }
                    break;
                case 425://Send character
                    {
                        string name = dataReader.GetString();
                        NetDataWriter writer = new NetDataWriter();
                        CharacterInformation charinfo = CharacterMethods.GetCharacterAsCharacterInfoPacket(name);
                        writer.Put((ushort)2016);
                        writer.PutPacketStruct(charinfo);
                        fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                        break;
                    }
                case 426:  // send all chars
                    {
                        NetDataWriter writer = new NetDataWriter();
                        CharacterInformationArray charinfo = CharacterMethods.GetAllCharactersAsCharacterInfoPackets();
                        writer.Put((ushort)2017);
                        writer.PutPacketStruct(charinfo);
                        fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                        break;
                    }
                case 88:
                {
                }
                    break;
                case 470: //setting avatar
                {
                    string name = dataReader.GetString();
                    int id = dataReader.GetInt();
                    if (util.GetNetworkUserFromPeer(fromPeer).User.Username == name)
                    {
                        UserMethods.SetAvatar(name, id);
                    }
                }
                break;

                case 471://setting status text
                    {
                        string name = dataReader.GetString();
                        string id = dataReader.GetString();
                        if (util.GetNetworkUserFromPeer(fromPeer).User.Username == name)
                        {
                            UserMethods.SetStatusText(name, id);
                        }
                    }
                    break;
                case 472: //adding user after accepting request
                    {
                        string target = dataReader.GetString();
                        User targetUser = UserMethods.GetUserByUsername(target);
                        NetworkUser targetNetworkUser = util.GetNetworkUserFromUser(targetUser);
                        NetworkUser me = util.GetNetworkUserFromPeer(fromPeer);
                        
                        UserMethods.AddFriend(me.User, targetUser);
                        UserMethods.AddFriend(targetUser, me.User);
                        UserMethods.RemoveRequest(me.User, targetUser);
                        UserMethods.RemoveRequest(targetUser, me.User);
                        if(targetNetworkUser != null)
                        {
                            SendNetworkEvent(targetUser, DeliveryMethod.ReliableOrdered, 3006, UserMethods.GetUserAsProfilePartInfoPacket(targetNetworkUser.User));
                        }
                        SendNetworkEvent(me, DeliveryMethod.ReliableOrdered, 3006, UserMethods.GetUserAsProfilePartInfoPacket(me.User));
                    }
                    break;
                case 473: //decling request / dismissing notification
                    {

                    }
                    break;
                case 474: //removing friend
                    {

                    }
                    break;
                case 475: //remove request
                    {
                    }
                    break;
                case 476:
                    {
                        string name = dataReader.GetString();
                        string cmd = dataReader.GetString();
                        NetworkUser user = util.GetNetworkUserFromPeer(fromPeer);
                        if (validation.ValidateNetworkUser(fromPeer, user))
                        {
                            Commands.ProcessCommand(user.User, cmd);
                        }
                    } break;
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
                            writer.Put("localhost");
                            writer.Put(gameServer.Value.masterKey);
                            writer.Put(gameServer.Value.port);

                            fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                            writer.Reset();
                        }
                    }
                }
                break;
                case 888: //Receive message from client
                    {
                        SendMessage sendMessage = dataReader.GetPacketStruct<SendMessage>();
                        User user = NetworkUsers.Where(usr => usr.Peer == fromPeer).FirstOrDefault().User;
                        User receivingUser = UserMethods.GetUserByUsername(sendMessage.ReceivingUser);
                        UserMethods.SaveMessageToDatabase(user.UserId, receivingUser.UserId, sendMessage.MessageText, sendMessage.TimeStamp);
                        //See if user is online
                        //util.GetNetworkUserFromUsername(sendMessage.ReceivingUser);
                    }
                    break;

                case 3010: //set character owned state of user frompeer after validation
                    {
                        CharacterOwned packet = dataReader.GetPacketStruct<CharacterOwned>();
                        bool success = false;
                        
                        if(validation.ValidateSender(fromPeer, packet.OwnerUsername))
                        {
                            User user = NetworkUsers.Where(p => p.Peer == fromPeer).FirstOrDefault().User;
                            Character character = CharacterMethods.GetCharacterByReferenceId(packet.Id);
                            if (packet.State) //purchase
                            {
                                if (packet.PremiumPayment)
                                {
                                    if (character.PremiumPrice <= user.PremiumBalance)
                                    {
                                        UserMethods.AddPremiumBalance(user, -character.Price);
                                        success = true;
                                    }
                                }
                                if (!packet.PremiumPayment)
                                {
                                    if (character.Price <= user.Balance)
                                    {
                                        UserMethods.AddBalance(user, -character.Price);
                                        success = true;
                                    }
                                }
                                if (success)
                                {
                                    UserMethods.SetCharacterOwnedState(user, character.ReferenceId, true);
                                    SendNetworkEvent(user, DeliveryMethod.ReliableOrdered, 3007, UserMethods.GetUserAsProfilePartInfoPacket(user));
                                }
                            }
                            else //refund check history 
                            {

                            }
                        }
                    }
                    break;
            }

            dataReader.Recycle();
        }
        
        public void SendConsoleMessage(User target, string message)
        {
            SendNetworkEvent(util.GetNetworkUserFromUser(target), 20000, DeliveryMethod.Unreliable, message);
        }

        private void SendNetworkEvent(NetworkUser target, ushort msgid, DeliveryMethod dm, string msg)
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(msgid);
            writer.Put(msg);
            target.Peer.Send(writer, dm);
        }

        private void StartGameServer(string serverName, int port, string masterKey, byte roomType, byte maxPlayers)
        {
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\GameServer\\FL_Game_Server.dll");
            string pathToFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                "\\GameServer\\FL_Game_Server.dll";
            Console.WriteLine($"Starting game server from: {pathToFile} with args:\n{pathToFile} {port} {masterKey} {roomType} {serverName} {maxPlayers}");
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"\"{pathToFile}\" {port} {masterKey} {roomType} {serverName} {maxPlayers}",
                        UseShellExecute = false,
                        CreateNoWindow = false,
                    }
                }; Console.WriteLine(process.Start());
                //process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not start program " + ex);
            }
        }

        private static void OnListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo info)
        {
            Console.WriteLine($"peer disconnected: {peer.EndPoint} user: {Instance.util.GetNetworkUserFromPeer(peer).User.Username}");
            Instance.NetworkUsers.Remove(Instance.util.GetNetworkUserFromPeer(peer));
        }

        public void SendNetworkEvent<T>(NetworkUser target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetDataWriter writer = new NetDataWriter();
            writer.Put(msgid);
            writer.PutPacketStruct(packet);
            target.Peer.Send(writer, dm);
        }

        public void SendNetworkEvent<T>(User target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetworkUser user = util.GetNetworkUserFromUser(target);
            if (user != null)
                SendNetworkEvent(user, dm, msgid, packet);
        }

        public void SendNetworkEvent<T>(NetPeer target, DeliveryMethod dm, ushort msgid, T packet) where T : struct
        {
            NetworkUser user = util.GetNetworkUserFromPeer(target);
            if (user != null)
                SendNetworkEvent(user, dm, msgid, packet);
        }
    }
}