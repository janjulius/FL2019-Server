using System;
using System.Collections.Generic;
using System.Threading;
using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;

namespace FL_Game_Server
{
    public class GameServer
    {
        private int serverPort = 10000;
        private bool stopServer = false;
        private int serverFreq = 10;
        private int maxConnections = 8;

        private EventBasedNetListener listener;
        private NetManager server;

        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public Dictionary<int, NetworkObject> NetworkObjects = new Dictionary<int, NetworkObject>();
        
        public Random rand = new Random();

        public void Run(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(args[0]);
                serverPort = int.Parse(args[0]);

                listener = new EventBasedNetListener();
                server = new NetManager(listener);

                try
                {
                    server.Start(serverPort);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    stopServer = true;
                    //TODO logic when gameserver fails
                }

                listener.ConnectionRequestEvent += GetConnectionRequest;
                listener.PeerDisconnectedEvent += PeerDisconnect;
                listener.PeerConnectedEvent += PeerConnected;
                listener.NetworkReceiveEvent += ReceivePackage;

                while (!stopServer)
                {
                    server.PollEvents();
                    Thread.Sleep(serverFreq);
                }

                server.Stop();
            }
        }

        private void GetConnectionRequest(ConnectionRequest request)
        {
            if (server.PeersCount < maxConnections)
                request.AcceptIfKey("SomeConnectionKey");
            else
                request.Reject();
        }

        private void PeerConnected(NetPeer peer)
        {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);

            
            NetDataWriter writer = new NetDataWriter();
            writer.Put((ushort) 1);
            writer.Put(peer.Id);

            int newPlayerId;
            do
            {
                newPlayerId = rand.Next(1000000, 9999999);
            } while (Players.ContainsKey(newPlayerId));

            writer.Put(newPlayerId);
            writer.Put(Players.Count == 0);
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
            writer.Reset();
            foreach (var player in Players)
            {
                writer.Reset();

                player.Value.SendNewPlayerData(writer);
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
            }

            foreach (var networkObject in NetworkObjects)
            {
                writer.Reset();
                networkObject.Value.SendObjectData(writer);
                peer.Send(writer, DeliveryMethod.ReliableOrdered);
            }
        }

        private void PeerDisconnect(NetPeer peer, DisconnectInfo disconnectinfo)
        {
            List<int> playersToRemove = new List<int>();
            List<int> objectsToRemove = new List<int>();
            foreach (var player in Players)
            {
                if (player.Value.peer == peer)
                {
                    playersToRemove.Add(player.Key);
                    foreach (var networkObject in NetworkObjects)
                    {
                        if (networkObject.Value.playerId == player.Key)
                        {
                            objectsToRemove.Add(networkObject.Key);
                        }
                    }
                }
            }

            foreach (var i in playersToRemove)
            {
                Players.Remove(i);
            }

            foreach (var i in objectsToRemove)
            {
                NetworkObjects.Remove(i);
            }


            NetDataWriter writer = new NetDataWriter();
            writer.Put((ushort) 3);
            writer.Put(peer.Id);
            SendOthers(peer, writer, DeliveryMethod.ReliableOrdered);
        }

        private void ReceivePackage(NetPeer peer, NetPacketReader dataReader, DeliveryMethod deliverymethod)
        {
        ushort msgid = dataReader.GetUShort();

            NetDataWriter writer = new NetDataWriter();

            switch (msgid)
            {
                case 1:
                    var playerId = dataReader.GetInt();
                    var pName = dataReader.GetString();
                    var isHost = dataReader.GetBool();
                    var charId = dataReader.GetInt();
                    var pColorR = dataReader.GetFloat();
                    var pColorG = dataReader.GetFloat();
                    var pColorB = dataReader.GetFloat();
                    var player = new Player(peer, playerId, isHost, pName, charId, pColorR, pColorG, pColorB);
                    Players.Add(playerId, player);
                    player.SendNewPlayerData(writer);
                    SendOthers(peer, writer, DeliveryMethod.ReliableOrdered);

                    break;
                case 4:
                    var playerUpdateId = dataReader.GetInt();
                    Players[playerUpdateId].ReadPlayerUpdate(dataReader);
                    Players[playerUpdateId].SendNewPlayerUpdate(writer);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    break;
                case 101: //create networkObject

                    int objectId;

                    do
                    {
                        objectId = rand.Next(1000000, 9999999);
                    } while (NetworkObjects.ContainsKey(objectId));

                    var netObj = new NetworkObject(
                        peer,
                        dataReader.GetInt(),
                        objectId,
                        dataReader.GetInt(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat(),
                        dataReader.GetFloat());
                    NetworkObjects.Add(objectId, netObj);

                    netObj.SendObjectData(writer);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    break;

                case 102:
                    var objectToDelete = dataReader.GetInt();
                    NetworkObjects.Remove(objectToDelete);

                    writer.Put((ushort) 102);
                    writer.Put(objectToDelete);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    break;
                case 103:
                    var objectToUpdate = dataReader.GetInt();
                    if (NetworkObjects.ContainsKey(objectToUpdate))
                    {
                        NetworkObjects[objectToUpdate].ReadData(dataReader);
                        NetworkObjects[objectToUpdate].WriteData(writer);
                        SendOthers(peer, writer, DeliveryMethod.Unreliable);
                    }

                    break;
                case 201:
                    byte[] data = new byte[dataReader.UserDataSize];
                    Array.Copy(dataReader.RawData, dataReader.UserDataOffset, data, 0, dataReader.UserDataSize);
                    var target = dataReader.GetByte();
                    var rpcName = dataReader.GetString();
                    var rpcObjectId = dataReader.GetInt();
                    switch (target)
                    {
                        case 0:
                            NetworkObjects[rpcObjectId].peer.Send(data, DeliveryMethod.ReliableUnordered);
                            break;
                        case 1:
                            SendOthers(NetworkObjects[rpcObjectId].peer, data, DeliveryMethod.ReliableUnordered);
                            break;
                        case 2:
                            server.SendToAll(data, DeliveryMethod.ReliableUnordered);
                            break;
                    }

                    break;

                case 301:
                    var levelId = dataReader.GetInt();
                    writer.Put((ushort) 301);
                    writer.Put(levelId);

                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);

                    NetworkObjects.Clear();
                    break;
            }

            dataReader.Recycle();
        }
        
        private void SendOthers(NetPeer peer, NetDataWriter writer, DeliveryMethod deliveryMethod)
        {
            foreach (NetPeer netPeer in server.ConnectedPeerList)
            {
                if (peer == netPeer) continue;

                netPeer.Send(writer, deliveryMethod);
            }
        }
        
        private void SendOthers(NetPeer peer, byte[] data, DeliveryMethod deliveryMethod)
        {
            foreach (NetPeer netPeer in server.ConnectedPeerList)
            {
                if (peer == netPeer) continue;

                netPeer.Send(data, deliveryMethod);
            }
        }
    }
}