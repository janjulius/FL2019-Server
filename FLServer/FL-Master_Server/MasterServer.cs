﻿using FL_Master_Server.User;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Authentication;
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
        Dictionary<int, NetPeer[]> playersWaiting = new Dictionary<int, NetPeer[]>();

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
            StartGameServer("testServerName", 10000, "SomeConnectionKey", 0);

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

            ulong msgid = reader.GetULong();

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


            if (msgid == 423) //load into master server PACKETS:PROFILEPARTINFO
            {
                string id = dataReader.GetString();
                string pwd = Security.GetHashString(dataReader.GetString());
                Console.WriteLine($"Got a conection from UniquePlayer: {id}");
                Console.WriteLine($"Verifying the user {id}({id.Length}):{pwd}({pwd.Length})");
                string[] friends = UserMethods.GetFriends(id);
                if (!UserAuth.VerifyPassword(id, pwd))
                {
                    Console.WriteLine("Authetication failed disconnectin the user");
                    fromPeer.Disconnect();
                }
                else
                {
                    NetDataWriter writer = new NetDataWriter();
                    FLServer.Models.User u = UserMethods.GetUserByUsername(id);
                    writer.Put((ushort) 2004);
                    writer.Put(u.Balance);
                    writer.Put(u.PremiumBalance);
                    writer.Put(u.Username);
                    writer.Put(u.Avatar);
                    writer.Put(u.Level);
                    writer.Put(u.Exp);
                    writer.PutArray(friends);
                    fromPeer.Send(writer, DeliveryMethod.Unreliable);
                }
            }
            else if (msgid == 424) //get player profile PACKETS:PROFILEACCOUNTINFO
            {
                string id = dataReader.GetString();
                NetDataWriter writer = new NetDataWriter();
                FLServer.Models.User u = UserMethods.GetUserByUsername(id);
                if (u != null) //player exists
                {
                    writer.Put((ushort) 2005);
                    writer.Put(u.Username);
                    writer.Put(u.Avatar);
                    writer.Put(u.Level);
                    writer.Put(u.Exp);
                    writer.Put(u.LastOnline.ToString());
                    fromPeer.Send(writer, DeliveryMethod.Unreliable);
                }
                else
                {
                    writer.Put((ushort) 2006);
                    writer.Put(Constants.CantFindProfile);
                    fromPeer.Send(writer, DeliveryMethod.Unreliable);
                }
            }
            else if (msgid == 88)
            {
            }
            else
            {
            }

            dataReader.Recycle();
        }

        private void StartGameServer(string serverName, int port, string masterKey, byte roomType)
        {
            //Console.WriteLine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)+"\\GameServer\\FL_Game_Server.dll");
            string pathToFile = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) +
                                "\\GameServer\\FL_Game_Server.dll";
            try
            {
                var process = new Process()
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"{pathToFile} {port} {masterKey} {roomType} {serverName}",
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