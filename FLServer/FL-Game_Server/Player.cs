using LiteNetLib;
using LiteNetLib.Utils;
using Shared.Extensions;

namespace FL_Game_Server
{
    public class Player
    {
        public NetPeer peer;

        public Packets.PlayerInfo playerInfo;


        public Player(NetPeer peer, Packets.PlayerInfo playerInfo)
        {
            this.peer = peer;
            this.playerInfo = playerInfo;
        }

        public void SendNewPlayerData(NetDataWriter writer)
        {
            writer.Put((ushort) 2);
            writer.PutBytesWithLength(playerInfo.ToByteArray());
        }

        public void ReadPlayerUpdate(Packets.PlayerInfo playerData)
        {
            playerInfo = playerData;
        }

        public void SendNewPlayerUpdate(NetDataWriter writer)
        {
            writer.Put((ushort) 4);
            writer.PutBytesWithLength(playerInfo.ToByteArray());
        }
    }
}