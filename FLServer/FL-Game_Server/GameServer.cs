using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net;
using System.Threading;
using FLServer.Models;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Extensions;
using FL_Game_Server.Packets;
using Shared.Packets;
using Shared.Users;

namespace FL_Game_Server
{
    public class GameServer
    {
        private int serverPort = 10000;
        private bool stopServer = false;
        private int serverFreq = 10;
        private int maxConnections = 4;
        private string masterKey;
        private byte roomType;
        private string serverName;
        private GameState inGame = GameState.InLobby;
        private int playersLeftAlive = 0;
        private int playersLoadedLevel = 0;
        private Messages msgs = new Messages(new Message[25]);


        private EventBasedNetListener listener;
        private NetManager server;

        public Dictionary<int, Player> Players = new Dictionary<int, Player>();
        public Dictionary<int, DateTime> playersWaitingToRespawn = new Dictionary<int, DateTime>();
        public Dictionary<int, NetworkObject> NetworkObjects = new Dictionary<int, NetworkObject>();

        public List<Damage> damagePackets = new List<Damage>();

        public Random rand = new Random();


        enum GameState
        {
            InLobby = 0,
            InGame = 1,
            InPostGame = 2,
        }

        public void Run(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(args[0]);
                serverPort = int.Parse(args[0]);
                masterKey = args[1];
                roomType = byte.Parse(args[2]);
                serverName = args[3];
                maxConnections = byte.Parse(args[4]);
                listener = new EventBasedNetListener();
                server = new NetManager(listener);

                server.UnconnectedMessagesEnabled = true;
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
                listener.NetworkReceiveUnconnectedEvent += ReceiveUnconnectedMessage;

                NetDataWriter UWriter = new NetDataWriter();

                UWriter.Put((ushort) 1);
                UWriter.Put(serverPort);
                UWriter.Put(masterKey);
                UWriter.Put(roomType);
                UWriter.Put((byte) maxConnections);
                UWriter.Put(serverName);

                SendMaster(UWriter);

                UWriter.Reset();

                while (!stopServer)
                {
                    if (playersWaitingToRespawn.Count != 0)
                    {
                        List<int> playersToRemove = new List<int>();
                        foreach (var player in playersWaitingToRespawn)
                        {
                            if ((DateTime.UtcNow - player.Value).TotalSeconds > 2d)
                            {
                                playersToRemove.Add(player.Key);
                            }
                        }

                        foreach (var i in playersToRemove)
                        {
                            playersWaitingToRespawn.Remove(i);
                            NetDataWriter writer = new NetDataWriter();
                            writer.Put((ushort) 154);
                            writer.Put(i);
                            Players[i].peer.Send(writer, DeliveryMethod.ReliableOrdered);
                            writer.Reset();
                        }

                        playersToRemove.Clear();
                    }


                    server.PollEvents();
                    Thread.Sleep(serverFreq);
                }

                UWriter.Put((ushort) 2);
                UWriter.Put(serverPort);

                SendMaster(UWriter);

                server.Stop();
            }
        }

        private void ReceiveUnconnectedMessage(IPEndPoint remoteendpoint, NetPacketReader reader, UnconnectedMessageType messagetype)
        {
            ushort msgid = reader.GetUShort();

            switch (msgid)
            {
                case 1:
                {
                    int playerId = reader.GetInt();
                    int playerLevel = reader.GetInt();
                    string playerRank = reader.GetString();
                    Players[playerId].playerInfo.playerLevel = playerLevel;
                    Players[playerId].playerInfo.playerRank = playerRank;
                    NetDataWriter writer = new NetDataWriter();
                    Players[playerId].SendNewPlayerUpdate(writer);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

            }
        }

        private void GetConnectionRequest(ConnectionRequest request)
        {
            if (server.PeersCount < maxConnections && inGame != GameState.InGame)
                request.AcceptIfKey(masterKey);
            else
                request.Reject();
        }

        private void PeerConnected(NetPeer peer)
        {
            Console.WriteLine("We got connection: {0}", peer.EndPoint);

            NetDataWriter UWriter = new NetDataWriter();

            UWriter.Put((ushort) 3);
            UWriter.Put(serverPort);

            SendMaster(UWriter);

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
            NetDataWriter UWriter = new NetDataWriter();

            UWriter.Put((ushort) 4);
            UWriter.Put(serverPort);

            SendMaster(UWriter);

            List<int> playersToRemove = new List<int>();
            List<int> objectsToRemove = new List<int>();
            foreach (var player in Players)
            {
                if (player.Value.peer == peer)
                {
                    UWriter.Reset();
                    UWriter.Put((ushort) 6);
                    UWriter.Put(serverPort);
                    UWriter.Put(player.Value.playerInfo.playerName);
                    SendMaster(UWriter);
                    
                    if (roomType == 0)
                    {
                        if (player.Value.playerInfo.isHost)
                        {
                            stopServer = true;
                        }
                    }

                    playersToRemove.Add(player.Key);
                    foreach (var networkObject in NetworkObjects)
                    {
                        if (networkObject.Value.objectData.playerId == player.Key)
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
                {
                    var player = new Player(peer, dataReader.GetBytesWithLength().ToStructure<PlayerInfo>());
                    Players.Add(player.playerInfo.playerId, player);
                    player.SendNewPlayerData(writer);
                    SendOthers(peer, writer, DeliveryMethod.ReliableOrdered);


                    NetDataWriter UWriter = new NetDataWriter();
                    UWriter.Put((ushort) 5);
                    UWriter.Put(serverPort);
                    UWriter.Put(player.playerInfo.playerId);
                    UWriter.Put(player.playerInfo.playerName);
                    SendMaster(UWriter);
                }
                    break;
                case 4:
                    var playerInfo = dataReader.GetBytesWithLength().ToStructure<PlayerInfo>();

                    Players[playerInfo.playerId].ReadPlayerUpdate(playerInfo);
                    Players[playerInfo.playerId].SendNewPlayerUpdate(writer);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    break;
                case 101: //create networkObject
                {
                    int objectId;
                    int lobjectId = dataReader.GetUShort();
                    ObjectData objectData = dataReader.GetBytesWithLength().ToStructure<ObjectData>();

                    do
                    {
                        objectId = rand.Next(1000000, 9999999);
                    } while (NetworkObjects.ContainsKey(objectId));

                    objectData.objectId = objectId;

                    var netObj = new NetworkObject(peer, objectData);
                    NetworkObjects.Add(objectId, netObj);

                    netObj.SendObjectData(writer);
                    writer.Put(lobjectId);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 102:
                {
                    var objectToDelete = dataReader.GetInt();
                    NetworkObjects.Remove(objectToDelete);

                    writer.Put((ushort) 102);
                    writer.Put(objectToDelete);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;
                case 103:
                {
                    int objectId = dataReader.GetInt();
                    ObjectPositionData objectData = dataReader.GetBytesWithLength().ToStructure<ObjectPositionData>();
                    if (NetworkObjects.ContainsKey(objectId))
                    {
                        NetworkObjects[objectId].ReadData(objectData);
                        NetworkObjects[objectId].WriteData(writer);
                        SendOthers(peer, writer, DeliveryMethod.Unreliable);
                    }
                }
                    break;

                case 104:
                {
                    writer.Put((ushort) 104);
                    writer.Put(dataReader.GetRemainingBytes());
                    SendOthers(peer, writer, DeliveryMethod.ReliableUnordered);
                }
                    break;

                case 150:
                {
                    var cooldownBytes = dataReader.GetBytesWithLength();
                    var cooldownData = cooldownBytes.ToStructure<Cooldown>();

                    writer.Put((ushort) 150);
                    writer.PutBytesWithLength(cooldownBytes);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 151:
                {
                    var damageBytes = dataReader.GetBytesWithLength();
                    var damageData = damageBytes.ToStructure<Damage>();

                    writer.Put((ushort) 151);
                    writer.PutBytesWithLength(damageBytes);
                    Players[damageData.damageTakerId].peer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 152:
                {
                    var damageBytes = dataReader.GetBytesWithLength();
                    var damageData = damageBytes.ToStructure<Damage>();
                    damagePackets.Add(damageData);

                    switch (damageData.damageType)
                    {
                        case 50: //case damage blocked
                        {
                            Players[damageData.damageTakerId].playerInfo.playerStats.damageBlocked += damageData.damage;
                            Players[damageData.damageDealerId].playerInfo.playerStats.damageMissed += damageData.damage;
                            Players[damageData.damageDealerId].playerInfo.gameInfo.ultCharge += 5;
                        }
                            break;
                        case 51: //case damage healed
                        {
                            Players[damageData.damageTakerId].playerInfo.playerStats.damageHealed += damageData.damage;
                            Players[damageData.damageTakerId].playerInfo.gameInfo.damage -= damageData.damage;
                        }
                            break;

                        default:
                        {
                            Players[damageData.damageDealerId].playerInfo.playerStats.damageDone += damageData.damage;
                            Players[damageData.damageTakerId].playerInfo.playerStats.damageTaken += damageData.damage;
                            Players[damageData.damageTakerId].playerInfo.gameInfo.damage += damageData.damage;
                            Players[damageData.damageDealerId].playerInfo.gameInfo.ultCharge += 15;
                        }
                            break;
                    }

                    if (Players[damageData.damageDealerId].playerInfo.gameInfo.ultCharge >= 100)
                        Players[damageData.damageDealerId].playerInfo.gameInfo.ultCharge = 100;

                    writer.Put((ushort) 152);
                    writer.Put(damageData.damageTakerId);
                    writer.PutBytesWithLength(Players[damageData.damageTakerId].playerInfo.gameInfo.ToByteArray());
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);

                    writer.Reset();

                    writer.Put((ushort) 152);
                    writer.Put(damageData.damageDealerId);
                    writer.PutBytesWithLength(Players[damageData.damageDealerId].playerInfo.gameInfo.ToByteArray());
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 153:
                {
                    int playerId = dataReader.GetInt();
                    Players[playerId].playerInfo.gameInfo.lives--;
                    Players[playerId].playerInfo.playerStats.deaths++;
                    Players[playerId].playerInfo.gameInfo.ultCharge = (short) (Players[playerId].playerInfo.gameInfo.ultCharge / 2);

                    if (Players[playerId].playerInfo.playerStats.highestDamageSurvived < Players[playerId].playerInfo.gameInfo.damage)
                        Players[playerId].playerInfo.playerStats.highestDamageSurvived = Players[playerId].playerInfo.gameInfo.damage;

                    Players[playerId].playerInfo.gameInfo.damage = 0;

                    var currentTime = DateTime.UtcNow;

                    for (int i = damagePackets.Count - 1; i >= 0; i--)
                    {
                        if (damagePackets[i].damageTakerId == playerId)
                        {
                            if ((currentTime - DateTime.FromBinary(damagePackets[i].timeStamp)).TotalSeconds < 15d)
                            {
                                Players[damagePackets[i].damageDealerId].playerInfo.playerStats.kills++;
                                Console.WriteLine($"player {Players[damagePackets[i].damageDealerId].playerInfo.playerName} killed player {Players[playerId].playerInfo.playerName}");
                            }

                            i = -1;
                        }
                    }

                    bool endGame = false;

                    if (Players[playerId].playerInfo.gameInfo.lives == 0)
                    {
                        Players[playerId].playerInfo.playerPlace = playersLeftAlive;
                        playersLeftAlive--;

                        if (playersLeftAlive <= 1)
                        {
                            foreach (var player in Players)
                            {
                                if (player.Value.playerInfo.gameInfo.lives > 0)
                                {
                                    player.Value.playerInfo.playerPlace = 1;
                                }
                            }

                            endGame = true;
                        }
                    }
                    else
                    {
                        playersWaitingToRespawn.Add(playerId, DateTime.UtcNow);
                    }

                    writer.Put((ushort) 152);
                    writer.Put(playerId);
                    writer.PutBytesWithLength(Players[playerId].playerInfo.gameInfo.ToByteArray());
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                    writer.Reset();

                    if (endGame)
                    {
                        inGame = GameState.InPostGame;

                        //to post game
                        writer.Put((ushort) 300);
                        writer.Put(Players.Count);
                        foreach (var player in Players)
                        {
                            writer.PutBytesWithLength(player.Value.playerInfo.ToByteArray());
                        }


                        server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                        
                        
                        NetDataWriter UWriter = new NetDataWriter();
                        UWriter.Put((ushort) 7);
                        UWriter.Put(serverPort);
                        UWriter.Put(Players.Count);
                        foreach (var player in Players)
                        {
                            UWriter.Put(player.Value.playerInfo.playerPlace);
                            UWriter.Put(player.Value.playerInfo.playerName);
                        }
                        SendMaster(UWriter);
                    }
                }
                    break;

                case 154:
                {
                    int playerId = dataReader.GetInt();
                    short ultcharge = dataReader.GetShort();
                    Players[playerId].playerInfo.gameInfo.ultCharge = ultcharge;
                    Players[playerId].playerInfo.playerStats.ultsUsed++;
                    writer.Put((ushort) 152);
                    writer.Put(playerId);
                    writer.PutBytesWithLength(Players[playerId].playerInfo.gameInfo.ToByteArray());
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 201:
                {
                    byte[] data = new byte[dataReader.UserDataSize];
                    Array.Copy(dataReader.RawData, dataReader.UserDataOffset, data, 0, dataReader.UserDataSize);
                    var target = dataReader.GetByte();
                    var rpcName = dataReader.GetString();
                    var rpcObjectId = dataReader.GetInt();
                    if (NetworkObjects.ContainsKey(rpcObjectId))
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
                }
                    break;

                case 300:
                {
                    inGame = GameState.InPostGame;

                    //to post game
                    writer.Put((ushort) 300);
                    writer.Put(Players.Count);
                    foreach (var player in Players)
                    {
                        writer.PutBytesWithLength(player.Value.playerInfo.ToByteArray());
                    }


                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 301:
                {
                    var levelId = dataReader.GetInt();

                    if (levelId == -2)
                        inGame = GameState.InLobby;
                    else if (levelId == -3)
                        inGame = GameState.InPostGame;
                    else
                    {
                        inGame = GameState.InGame;
                        playersLeftAlive = Players.Count;


                        foreach (var player in Players)
                        {
                            player.Value.playerInfo.playerStats = new PlayerStats();
                            player.Value.playerInfo.gameInfo.lives = 3;
                            player.Value.playerInfo.gameInfo.health = 0;
                            player.Value.playerInfo.gameInfo.damage = 0;
                            player.Value.playerInfo.gameInfo.ultCharge = 0;
                            writer.Put((ushort) 152);
                            writer.Put(player.Key);
                            writer.PutBytesWithLength(player.Value.playerInfo.gameInfo.ToByteArray());
                            server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                            writer.Reset();
                        }
                    }

                    writer.Put((ushort) 301);
                    writer.Put(levelId);


                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);

                    NetworkObjects.Clear();
                }
                    break;

                case 302:
                {
                    playersLoadedLevel++;

                    byte i = 0;
                    foreach (var player in Players)
                    {
                        player.Value.playerInfo.gameInfo.spawnPlace = i;
                        i++;

                        writer.Put((ushort) 152);
                        writer.Put(player.Key);
                        writer.PutBytesWithLength(player.Value.playerInfo.gameInfo.ToByteArray());
                        server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                        writer.Reset();
                    }


                    if (playersLoadedLevel == Players.Count)
                    {
                        writer.Put((ushort) 302);
                        server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                        playersLoadedLevel = 0;
                    }
                }
                    break;

                case 601: //receive message from lobby
                {
                    byte[] byteMessage = dataReader.GetBytesWithLength();
                    Message message = byteMessage.ToStructure<Message>();
                    for (int i = 0; i < msgs.AllMessages.Length; i++)
                     {
                            if (msgs.AllMessages[i].MessageText != "")
                            {
                                msgs.AllMessages[i] = message;
                                break;
                            }
                     }

                    writer.Put((ushort) 307);
                    writer.PutPacketStruct(message);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
                    break;

                case 602: //get message history
                {
                    writer.Put((ushort) 303);
                    writer.PutPacketStruct(msgs);
                    server.SendToAll(writer, DeliveryMethod.ReliableOrdered);
                }
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

        private void SendMaster(NetDataWriter writer)
        {
            IPAddress mServerAdress = IPAddress.Parse("127.0.0.1");
            IPEndPoint mServer = new IPEndPoint(mServerAdress, 9052);


            server.SendUnconnectedMessage(writer, mServer);
        }
    }
}