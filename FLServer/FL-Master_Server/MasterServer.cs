﻿using FL_Master_Server.Player;
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
using System.Runtime.InteropServices;
using System.Threading;
using FLServer.Models;
using FL_Master_Server.Player.Content;
using Shared.Packets.UserState;
using FL_Master_Server.Net;

namespace FL_Master_Server
{
    public class MasterServer
    {
        //singleton
        private static MasterServer instance = null;

        private MasterServer()
        {
        }

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

        private EventBasedNetListener listener;
        private NetManager server;

        private bool running = true;

        public List<NetworkUser> NetworkUsers = new List<NetworkUser>();

        Dictionary<int, GameServerInfo> GameServers = new Dictionary<int, GameServerInfo>();
        Dictionary<int, List<NetPeer>> playersWaiting = new Dictionary<int, List<NetPeer>>();

        Random random = new Random();

        private string MasterServerIP = "localhost";

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
                case 5:
                {
                    int serverPort = reader.GetInt();
                    int playerId = reader.GetInt();
                    string userName = reader.GetString();
                    GameServers[serverPort].players.Add(userName);
                    Console.WriteLine(userName + " player joined server " + serverPort);
                    
                    NetDataWriter UWriter = new NetDataWriter();
                    UWriter.Put((ushort) 1);
                    UWriter.Put(playerId);
                    UWriter.Put(UserMethods.GetLevel(userName));
                    UWriter.Put(UserMethods.GetRankOfUser(userName));
                    SendGameServer(UWriter,serverPort);
                }
                    break;
                case 6:
                {
                    int serverPort = reader.GetInt();
                    string userName = reader.GetString();
                    GameServers[serverPort].players.Remove(userName);
                    Console.WriteLine(userName + " player left server " + serverPort);
                }
                    break;
                case 7:
                {
                    int serverPort = reader.GetInt();
                    int totalPlayers = reader.GetInt();
                    for (int i = 0; i < totalPlayers; i++)
                    {
                        var place = reader.GetInt();
                        var name = reader.GetString();
                        Console.WriteLine("player " + name + " ended on " + place);
                        UserMethods.AddExp(name,120/place);
                        UserMethods.AddBalance(UserMethods.GetUserByUsername(name),200/place);
                        UserMethods.SetElo(UserMethods.GetUserByUsername(name), (int)Shared.Ranked.RankCalculator.GetNewELO(UserMethods.GetUserByUsername(name).RankedElo, 1250, place <= 2));
                    }

                    Console.WriteLine("a game has ended on " + serverPort);
                    
                }
                    break;

                case 500:
                    {
                        string username = reader.GetString();
                        var retreivedUser = Util.GetNetworkUser(username);

                        NetDataWriter writer = new NetDataWriter();
                        writer.Put((ushort)1);
                        writer.Put(retreivedUser == null ? false : true);
                        SendLogin(writer);
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
                    if (!UserAuth.VerifyPassword(id, pwd))
                    {
                        Console.WriteLine("Authetication failed disconnectin the user");
                        fromPeer.Disconnect();
                    }
                    else
                    {
                        NetDataWriter writer = new NetDataWriter();
                        FLServer.Models.User u = UserMethods.GetUserByUsername(id);

                        if (u.Rights < 0)
                        {
                            Console.WriteLine($"Disconnected banned user {u.Username}");
                            fromPeer.Disconnect();
                        }

                        NetworkUser me = Util.GetNetworkUser(fromPeer);
                        if (me != null)
                            me.User = u;
                        else
                            break; //user not found somehow not connected

                        writer.Put((ushort) 2004);
                        writer.PutPacketStruct(UserMethods.GetUserAsProfilePartInfoPacket(u));
                        fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                    }
                }
                    break;
                case 424:
                {
                    string id = dataReader.GetString();
                    NetDataWriter writer = new NetDataWriter();
                    ProfileAccountInfo pai = UserMethods.GetProfileAccountInfoPacket(id, Util.GetNetworkUser(fromPeer).User);

                    writer.Put((ushort) 2005);
                    writer.PutPacketStruct(pai);
                    fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;
                case 425: //Send character
                {
                    string name = dataReader.GetString();
                    NetDataWriter writer = new NetDataWriter();
                    CharacterInformation charinfo = CharacterMethods.GetCharacterAsCharacterInfoPacket(name);
                    writer.Put((ushort) 2016);
                    writer.PutPacketStruct(charinfo);
                    fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                    break;
                }

                case 426: // send all chars
                {
                    NetDataWriter writer = new NetDataWriter();
                    CharacterInformationArray charinfo = CharacterMethods.GetAllCharactersAsCharacterInfoPackets();
                    writer.Put((ushort) 2017);
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
                        NetworkUser nu = Util.GetNetworkUser(fromPeer);
                    if (nu.User.Username == name)
                    {
                        UserMethods.SetAvatar(name, id);
                    }
                    FriendSlotPacket[] friends = UserMethods.GetFriendsAsPacket(nu.User.Username);
                    foreach (FriendSlotPacket friend in friends)
                    {
                        Util.GetNetworkUser(friend.Name)?.RefreshInterface(RefreshableInterface.FriendsList);
                    }

                }
                    break;

                case 471: //setting status text
                {
                    string name = dataReader.GetString();
                    string id = dataReader.GetString();
                    NetworkUser nu = Util.GetNetworkUser(fromPeer);
                    if (Util.GetNetworkUser(fromPeer).User.Username == name)
                    {
                        UserMethods.SetStatusText(name, id);
                    }
                    FriendSlotPacket[] friends = UserMethods.GetFriendsAsPacket(nu.User.Username);
                    foreach (FriendSlotPacket friend in friends)
                    {
                        Util.GetNetworkUser(friend.Name)?.RefreshInterface(RefreshableInterface.FriendsList);
                    }
                }
                    break;
                case 472: //adding user after accepting request
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                    NetworkUser targetNetworkUser = Util.GetNetworkUser(targetUser);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);

                    UserMethods.AddFriend(me.User, targetUser);
                    UserMethods.AddFriend(targetUser, me.User);
                    UserMethods.RemoveRequest(me.User, targetUser, 0);
                    UserMethods.RemoveRequest(targetUser, me.User, 0);
                        targetNetworkUser?.RefreshInterface(RefreshableInterface.Client);
                        me.RefreshInterface(RefreshableInterface.Client);
                    }
                    break;
                case 473: //decling request / dismissing notification
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);

                    UserMethods.RemoveRequest(targetUser, me.User, 0);
                        me.RefreshInterface(RefreshableInterface.Client);
                 }
                    break;
                case 474: //removing friend
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                    NetworkUser targetNetworkUser = Util.GetNetworkUser(targetUser);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);

                    UserMethods.RemoveFriend(me.User, targetUser);
                    UserMethods.RemoveFriend(targetUser, me.User);
                        targetNetworkUser?.RefreshInterface(RefreshableInterface.FriendsList);

                        me.RefreshInterface(RefreshableInterface.FriendsList);
                    }
                    break;
                case 475: //remove request
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);

                        UserMethods.RemoveRequest(targetUser, me.User, 1);
                        me.RefreshInterface(RefreshableInterface.Client);
                }
                    break;
                case 476:
                {
                    string name = dataReader.GetString();
                    string cmd = dataReader.GetString();
                    NetworkUser user = Util.GetNetworkUser(fromPeer);
                    if (validation.ValidateNetworkUser(fromPeer, user))
                    {
                        Commands.ProcessCommand(user.User, cmd);
                    }
                }
                    break;
                case 477:
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                        NetworkUser tnu = Util.GetNetworkUser(targetUser);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);
                    if (targetUser != null)
                    {
                        UserMethods.CreateFriendRequest(me.User, targetUser);
                            tnu.RefreshInterface(RefreshableInterface.Client);
                        }
                }
                    break;
                case 478: //chat
                {
                    string target = dataReader.GetString();
                    User targetUser = UserMethods.GetUserByUsername(target);
                    NetworkUser me = Util.GetNetworkUser(fromPeer);
                    NetDataWriter writer = new NetDataWriter();
                    Messages msginfo = new Messages(UserMethods.GetLatestMessages(me.User, targetUser));
                    writer.Put((ushort) 890);
                    writer.PutPacketStruct(msginfo);
                    fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;
                case 479: //refreshing client packet
                    {
                        NetworkUser me = Util.GetNetworkUser(fromPeer);
                        me.RefreshInterface(RefreshableInterface.Client);
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
                            writer.Put(MasterServerIP);
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
                    byte[] byteMessage = dataReader.GetBytesWithLength();
                    Message message = byteMessage.ToStructure<Message>();
                    User me = NetworkUsers.Where(usr => usr.Peer == fromPeer).FirstOrDefault().User;
                    User target = UserMethods.GetUserByUsername(message.ReceivingUser);
                    NetDataWriter writer = new NetDataWriter();
                    if (target != null)
                    {
                        UserMethods.SaveMessageToDatabase(me.UserId, target.UserId, message.MessageText, message.TimeStamp);
                        if (Util.IsOnline(target))
                        {
                            writer.Put((ushort) 889);
                            writer.PutPacketStruct(message);

                            Util.GetNetworkUser(target).Peer.Send(writer, DeliveryMethod.ReliableOrdered);
                        }
                    }
                }
                    break;
                case 889: //get message history
                {
                    Message sendMessage = dataReader.GetPacketStruct<Message>();
                    User me = NetworkUsers.Where(usr => usr.Peer == fromPeer).FirstOrDefault().User;
                    User target = UserMethods.GetUserByUsername(sendMessage.ReceivingUser);
                    NetDataWriter writer = new NetDataWriter();
                    Messages msges = new Messages(UserMethods.GetLatestMessages(me, target));

                    writer.Put((ushort) 890);
                    writer.PutPacketStruct(msges);

                    fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;
                case 3010: //set character owned state of user frompeer after validation
                {
                    CharacterOwned packet = dataReader.GetPacketStruct<CharacterOwned>();
                    bool success = false;

                    if (validation.ValidateSender(fromPeer, packet.OwnerUsername))
                    {
                        User user = Util.GetNetworkUser(fromPeer).User;
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
                                    NetEvent.SendNetworkEvent(user, DeliveryMethod.ReliableOrdered, 3007, UserMethods.GetUserAsProfilePartInfoPacket(user));
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


        private void StartGameServer(string serverName, int port, string masterKey, byte roomType, byte maxPlayers)
        {
            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\GameServer\\FL_Game_Server.dll");
            string pathToFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                "\\GameServer\\FL_Game_Server.dll";
            if (!isLinux)
            {
                Console.WriteLine($"Starting game server from: {pathToFile} with args:\n{pathToFile} {port} {masterKey} {roomType} {serverName} {maxPlayers}");

                try
                {
                    var process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "dotnet",
                            Arguments = $"\"{pathToFile}\" {port} {masterKey} {roomType} \"{serverName}\" {maxPlayers}",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };
                    Console.WriteLine(process.Start());
                    //process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not start program " + ex);
                }
            }
            else
            {
                pathToFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                             "/GameServer/FL_Game_Server.dll";
                Console.WriteLine($"Starting game server from: {pathToFile} with args:\n{pathToFile} {port} {masterKey} {roomType} \"{serverName}\" {maxPlayers}");

                try
                {
                    var process = new Process()
                    {
                        StartInfo = new ProcessStartInfo
                        {
                            FileName = "/bin/bash",
                            Arguments = $"-c \"dotnet {pathToFile} {port} {masterKey} {roomType} {serverName} {maxPlayers}\"",
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };
                    Console.WriteLine(process.StartInfo.Arguments);
                    Console.WriteLine(process.Start());
                    //process.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not start program " + ex);
                }
            }
        }

        private void SendLogin(NetDataWriter writer)
        {
            IPAddress mServerAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint mServer = new IPEndPoint(mServerAdress, 9050);
            
            server.SendUnconnectedMessage(writer, mServer);
        }

        private static void OnListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo info)
        {
            Console.WriteLine($"peer disconnected: {peer.EndPoint} user: {Util.GetNetworkUser(peer).User.Username}");
            Instance.NetworkUsers.Remove(Util.GetNetworkUser(peer));
        }
        
        
        
        private void SendGameServer(NetDataWriter writer,int serverport)
        {
            IPAddress mServerAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint mServer = new IPEndPoint(mServerAdress, serverport);


            server.SendUnconnectedMessage(writer, mServer);
        }
    }
}