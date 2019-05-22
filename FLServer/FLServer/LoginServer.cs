using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Authentication;
using Shared.General;
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
                General.SetNewVersion(Constants.ServerVersion);
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
                Console.WriteLine("We got connection: {0}", peer.EndPoint);
                NetDataWriter writer = new NetDataWriter(); 
                writer.Put((ushort)2003);         //this means the server is online
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
            };

            listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            listener.PeerDisconnectedEvent += OnListenerOnPeerDisconnectedEvent;

            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:{Constants.Port}");
            while (running)
            {
                server.PollEvents();
                Thread.Sleep(1000);
            }

            server.Stop();
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


            if (msgid == 3) //login
            {
                string u = dataReader.GetString();
                string p = dataReader.GetString();

                NetDataWriter response = new NetDataWriter();

                string a = Security.GetHashString(p);
                if (UserAuth.VerifyPassword(u, a))
                {
                    response.Put((ushort)2002); //succesful login
                    User user = UserMethods.GetUserByUsername(u);
                    string t = user.UniqueIdentifier;
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
    }
}
