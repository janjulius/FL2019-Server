using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FL_Patch_Server
{
    class PatchServer
    {
        private EventBasedNetListener listener;
        private NetManager server;

        private bool running = true;
        public void Run()
        {
            Console.WriteLine("Starting Patching server..");
            listener = new EventBasedNetListener();
            server = new NetManager(listener);
            Console.WriteLine("Attempting to run server");
            
            listener.NetworkReceiveEvent += OnListenerOnNetworkReceiveEvent;

            listener.ConnectionRequestEvent += request =>
            {
                if (server.PeersCount < 1000)
                    request.AcceptIfKey("patch");
                else
                    request.Reject();
            };

            listener.PeerConnectedEvent += peer =>
            {
                Console.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                //NetDataWriter writer = new NetDataWriter();                 // Create writer class
                //writer.Put("da050fd3-d662-4dd8-929e-0228f76b16a6");         // Put some string
                //peer.Send(writer, DeliveryMethod.ReliableOrdered);          // Send with reliability
            };


            try { server.Start(9051); } catch (Exception e) { Console.WriteLine(e); }
            Console.WriteLine($"Server started succesfully \n{server.IsRunning}:9051");
            while (running)
            {
                server.PollEvents();
                System.Threading.Thread.Sleep(15);
            }

            server.Stop();

        }

        private void OnListenerOnNetworkReceiveEvent(NetPeer fromPeer, NetPacketReader dataReader,
           DeliveryMethod deliveryMethod)
        {
            ushort msgid = dataReader.GetUShort();
            
            if (msgid == 1) //get version
            {
                var peerVersion = dataReader.GetString();
                string serverVersion;
                var response = new NetDataWriter();

                serverVersion = General.GetVersion();
                if (peerVersion == serverVersion)
                {
                    response.Put("patcher-1"); //succesful 
                }
                else
                    response.Put("patcher-2"); //bad 

                fromPeer.Send(response, DeliveryMethod.ReliableOrdered);
                response.Reset();
            }
            if(msgid == 2)
            {
                var response = new NetDataWriter();
                response.Put(General.GetVersion());
                fromPeer.Send(response, DeliveryMethod.ReliableOrdered);
                response.Reset();
            }

            dataReader.Recycle();
        }

        private void OnListenerOnConnectionRequestEvent(ConnectionRequest request)
        {
            Console.WriteLine("Onlistenernconnectionrequestevent");
            if (server.PeersCount < 1000)
            {
                request.AcceptIfKey("patch");
            }
            else
            {
                request.Reject();
            }
        }
    }
}
