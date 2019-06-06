using FL_Game_Server.Packets;
using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Extensions;

namespace FL_Game_Server
{
    public class NetworkObject
    {
        public NetPeer peer;
        public ObjectData objectData;


        public NetworkObject(NetPeer peer, ObjectData objectData)
        {
            this.peer = peer;
            this.objectData = objectData;
        }

        public void WriteData(NetDataWriter writer)
        {
            writer.Put((ushort) 103);
            writer.Put(objectData.objectId);
            writer.PutBytesWithLength(objectData.positionData.ToByteArray());
        }

        public void ReadData(ObjectPositionData objectData)
        {
            this.objectData.positionData = objectData;
        }

        public void SendObjectData(NetDataWriter writer)
        {
            writer.Put((ushort) 101);
            writer.Put(peer.Id);
            writer.PutBytesWithLength(objectData.ToByteArray());
        }
    }
}