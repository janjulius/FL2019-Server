using FL_Master_Server.User;
using LiteNetLib;
using System;
using System.Collections.Generic;
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

        public void Run()
        {
            Console.WriteLine("Starting Master server..");
            Console.WriteLine("Assigning serverlistener..");
            listener = new EventBasedNetListener();
            Console.WriteLine($"Serverlistener assigned: {listener.ToString()}");
            

            Console.WriteLine("Assigning NetManager with serverlistener");
            server = new NetManager(listener);
            Console.WriteLine("Attempting to run server");
            try { server.Start(Constants.Port); } catch (Exception e) { Console.WriteLine(e); }
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

            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:{Constants.Port}");
            while (running)
            {
                server.PollEvents();
                Thread.Sleep(15);
            }

            server.Stop();
        }

        private void OnListenerOnNetworkReceiveEvent(NetPeer fromPeer, NetPacketReader dataReader,
          DeliveryMethod deliveryMethod)
        {
            ushort msgid = dataReader.GetUShort();


            if (msgid == 423)
            {
                string id = dataReader.GetString();
                string pwd = dataReader.GetString();
                Console.WriteLine($"Got a conection from UniquePlayer: {id}");
                fromPeer.Disconnect();
            }
            else if (msgid == 88)
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
