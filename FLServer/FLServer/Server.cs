
using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
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
    class Server
    {
        private EventBasedNetListener listener;
        private const int Port = 9050;
        private NetManager server;

        delegate string HashDelegate(string a);
        HashDelegate myHashDelegate;

        private bool running = true;

        public void Run()
        {
            Console.WriteLine("Starting server..");
            Console.WriteLine("Assigning serverlistener..");
            listener = new EventBasedNetListener();
            Console.WriteLine($"Serverlistener assigned: {listener.ToString()}");

            Console.WriteLine("Assigning NetManager with serverlistener");
            server = new NetManager(listener);
            Console.WriteLine("Attempting to run server");
            try { server.Start(Port); } catch (Exception e) { Console.WriteLine(e); }
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
                writer.Put("da050fd3-d662-4dd8-929e-0228f76b16a6");         // Put some string
                peer.Send(writer, DeliveryMethod.ReliableOrdered);          // Send with reliability
            };

            listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            listener.PeerDisconnectedEvent += OnListenerOnPeerDisconnectedEvent;
            myHashDelegate += GetHashString;
            // listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:{Port}");
            while (running)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }

            server.Stop();
        }

        internal ProgramResult AddNewUser(string name, string password, string email)
        {
            using (var ctx = new FLDBContext())
            {
                if (!UserExists(name))
                {

                    ctx.User.Add(new User()
                    {
                        Username = name,
                        Level = 99,
                        Password = password,
                        CreationDate = DateTime.UtcNow,
                        Email = email
                    });
                }
                ctx.SaveChanges();
            }
            return new ProgramResult(true, "User added");
        }

        private bool UserExists(string name)
        {
            using (var ctx = new FLDBContext())
            {
                var usr = ctx.User.Where(user => user.Username == name);

                if (usr.Any())
                    return true;
            }
            return false;
        }

        private bool VerifyPassword(string name, string password)
        {
            using (var ctx = new FLDBContext())
            {
                if (UserExists(name))
                {
                    return ctx.User.Where(u => u.Username == name).First().Password == password;
                }
            }
            return false;
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


        private void UpdateLastLogin(string username)
        {
            using (var ctx = new FLDBContext())
            {
                ctx.User.Where(u => u.Username == username).First().LastOnline = DateTime.UtcNow;
            }
        }

        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
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
                var hpw = GetHashString(password);
                AddNewUser(username, hpw, email);
            }
            else if (msgid == 3) //login
            {
                var u = dataReader.GetString();
                var p = dataReader.GetString();

                var response = new NetDataWriter();
                var a = GetHashString(p.Trim());
                if (VerifyPassword(u, a))
                {
                    response.Put("Welcome! your login was succesful");
                    UpdateLastLogin(u);
                }
                else
                    response.Put("Your credentials were incorrect or do not exist");

                fromPeer.Send(response, DeliveryMethod.ReliableOrdered);
                response.Reset();
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
                var acceptedPeer = request.AcceptIfKey("gamekey");
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
