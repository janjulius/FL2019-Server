using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Authentication;
using Shared.Security;
using Shared.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace FLServer
{
    class LoginServer
    {
        private EventBasedNetListener listener;
        private NetManager server;

        delegate string HashDelegate(string a);
        HashDelegate myHashDelegate;

        private bool running = true;

        public void Run()
        {
            Console.WriteLine("Starting Login server..");
            Console.WriteLine("Assigning serverlistener..");
            listener = new EventBasedNetListener();
            Console.WriteLine($"Serverlistener assigned: {listener.ToString()}");

            //update server version
            if (Constants.UpdateVersion)
            {
                Console.WriteLine("Updating server version.");
                Console.WriteLine(SetNewVersion(Constants.ServerVersion));
            }

            Console.WriteLine("Assigning NetManager with serverlistener");
            server = new NetManager(listener);
            Console.WriteLine("Attempting to run server");
            try { server.Start(Constants.Port); } catch (Exception e) { Console.WriteLine(e); }
            //if (!server.Start(Port))
            //{
            //    Console.WriteLine("Server start failed");
            //    Console.ReadKey();
            //    return;
            //}
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
                NetDataWriter writer = new NetDataWriter();                 // Create writer class
                writer.Put((ushort)2003);         // Put some string
                peer.Send(writer, DeliveryMethod.ReliableOrdered);          // Send with reliability
            };

            listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            listener.PeerDisconnectedEvent += OnListenerOnPeerDisconnectedEvent;
            myHashDelegate += Security.GetHashString;
            // listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:{Constants.Port}");
            while (running)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }

            server.Stop();
        }

        internal ProgramResult SetNewVersion(string versionNumber)
        {
            using (var ctx = new FLDBContext())
            {
                var n = ctx.ServerVersion.First().VersionNr;
                if (n == versionNumber)
                {
                    return new ProgramResult(true, $"Version already up to date ({versionNumber}).");
                }
                ctx.ServerVersion.First().VersionNr = versionNumber;
                ctx.SaveChanges();
                return new ProgramResult(true, $"Server version updated from {n} to {versionNumber}");
            }      
        }

        internal string GetVersion()
        {
            using (var ctx = new FLDBContext())
            {
                return ctx.ServerVersion.First().VersionNr;
            }
        }
        
        internal ProgramResult AddNewUser(string name, string password, string email)
        {
            using (var ctx = new FLDBContext())
            {
                if (!UserMethods.UserExists(name))
                {

                    ctx.User.Add(new User()
                    {
                        Username = name,
                        UniqueIdentifier = new Guid().ToString(),
                        Level = 0,
                        Password = password,
                        CreationDate = DateTime.UtcNow,
                        Email = email
                    });
                }
                ctx.SaveChanges();
            }
            return new ProgramResult(true, "User added");
        }

        internal ProgramResult AddFriend(string name, string toAdd)
        {
            using (var ctx = new FLDBContext())
            {
                var uId = ctx.User.Where(u => u.Username == name).First().UserId;
                var fId = ctx.User.Where(f => f.Username == toAdd).First().UserId;

                if (ctx.UserFriend.Where(a => a.UserId == uId && a.FriendId == fId).Any())
                    return new ProgramResult(false, "Friend already added");

                ctx.UserFriend.Add(
                    new UserFriend() {
                        UserId = uId, FriendId = fId
                    });
                ctx.SaveChanges();
            }
            return new ProgramResult(true, $"User {name} added {toAdd}");
        }

        internal ProgramResult GetFriends(string name)
        {
            using (var ctx = new FLDBContext())
            {
                var user = ctx.User.Where(u => u.Username == name).First();

                var res = ctx.UserFriend.Where(u => u.UserId == user.UserId).AsEnumerable();
                StringBuilder sb = new StringBuilder();

                foreach (var r in res)
                {
                    sb.Append(
                    ctx.User.Where(a => a.UserId == r.FriendId).First().Username + ",");
                }
                return new ProgramResult(true, sb.ToString());
            }
        }


        private string GetUniqueIdentifier(string u)
        {
            using (var ctx = new FLDBContext())
            {
                return ctx.User.Where(a => a.Username == u).First().UniqueIdentifier;
            }
        }

        public ProgramResult GetUserLvl(int level)
        {
            using (var ctx = new FLDBContext())
            {
                var result = ctx.User.Where(user => user.Level == level && user.Username.StartsWith('J'));

                foreach (var r in result)
                {
                    Console.WriteLine($"{r.Username}");

                }
                return new ProgramResult(true);
            }
        }


        

        

        private void OnListenerOnConnectionRequestEvent(ConnectionRequest request)
        {
            Console.WriteLine("Onlistenernconnectionrequestevent");
            if (server.PeersCount < Constants.MaxConnections)
            {
                request.AcceptIfKey(Constants.ConnectionKey);
            }
            else
            {
                request.Reject();
            }
        }

        private void OnListenerOnNetworkReceiveEvent(NetPeer fromPeer, NetPacketReader dataReader,
            DeliveryMethod deliveryMethod)
        {
            ushort msgid = dataReader.GetUShort();


            if (msgid == 2) { //register
                var username = dataReader.GetString();
                var password = dataReader.GetString();
                var email = dataReader.GetString();
                var hpw = Security.GetHashString(password);
                AddNewUser(username, hpw, email);
                dataReader.Recycle();
            }
            else if (msgid == 3) //login
            {
                var u = dataReader.GetString();
                var p = dataReader.GetString();
                
                var response = new NetDataWriter();

                var a = Security.GetHashString(p);
                if (UserAuth.VerifyPassword(u, a))
                {
                    response.Put((ushort)2002); //succesful login
                    string t = GetUniqueIdentifier(u);
                    response.Put(t);
                    UserMethods.UpdateLastLogin(u);
                }
                else
                {
                    response.Put((ushort)2001); //bad credentials
                }

                
                fromPeer.Send(response, DeliveryMethod.ReliableOrdered);
                response.Reset();
            }
            else if (msgid == 4) //Send server version
            {

            }
            else
            {

            }

            dataReader.Recycle();
        }


        private static void OnListenerOnPeerDisconnectedEvent(NetPeer peer, DisconnectInfo info)
        {
            Console.WriteLine($"peer disconnected: {peer.EndPoint}");
        }

        private class ServerListener : INetEventListener
        {
            public NetManager Server;

            public void OnPeerConnected(NetPeer peer)
            {
                Console.WriteLine("[Server] Peer connected: " + peer.EndPoint);
                var peers = Server.GetPeers(ConnectionState.Connected);
                foreach (var netPeer in peers)
                {
                    Console.WriteLine("ConnectedPeersList: id={0}, ep={1}", netPeer.Id, netPeer.EndPoint);
                }
            }

            public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
            {
                Console.WriteLine("[Server] Peer disconnected: " + peer.EndPoint + ", reason: " + disconnectInfo.Reason);
            }

            public void OnNetworkError(IPEndPoint endPoint, SocketError socketErrorCode)
            {
                Console.WriteLine("[Server] error: " + socketErrorCode);
            }

            public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, DeliveryMethod deliveryMethod)
            {
                //echo
                peer.Send(reader.GetRemainingBytes(), deliveryMethod);

                //fragment log
                if (reader.AvailableBytes == 13218)
                {
                    Console.WriteLine("[Server] TestFrag: {0}, {1}",
                        reader.RawData[reader.UserDataOffset],
                        reader.RawData[reader.UserDataOffset + 13217]);
                }
            }

            public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
            {
                Console.WriteLine("[Server] ReceiveUnconnected: {0}", reader.GetString(100));
            }

            public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
            {

            }

            public void OnConnectionRequest(ConnectionRequest request)
            {
                var acceptedPeer = request.AcceptIfKey("9292e1a2-9684-4fb3-98db-bef29fcec999");
                Console.WriteLine("[Server] ConnectionRequest. Ep: {0}, Accepted: {1}",
                    request.RemoteEndPoint,
                    acceptedPeer != null);
            }

            //public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
            //{
            //    throw new NotImplementedException();
            //}
            //
            //public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
            //{
            //    throw new NotImplementedException();
            //}
        }

        internal sealed class ProgramResult
        {
            public bool Result { get; set; }
            public string Info { get; set; }
            public ProgramResult(bool r)
            {
                Result = r;
            }
            public ProgramResult(string info)
            {
                Info = info;
            }
            public ProgramResult(bool r, string info)
            {
                Result = r;
                Info = info;
            }
            public override string ToString()
            {
                return Result ? $@"Success: {Info}" : $@"Something went wrong: {Info}";
            }
        }
    }
}
