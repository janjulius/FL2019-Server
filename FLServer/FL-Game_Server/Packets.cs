using System;
using System.Runtime.InteropServices;

namespace FL_Game_Server
{
    public static class Packets
    {
        [Serializable]
        public struct PlayerInfo
        {
            public int networkId;
            public int playerId;
            public bool isHost;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 100)]
            public string playerName;
            public int characterId;
            public int playerPlace;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3) ]
            public float[] playerColor;

            public PlayerInfo(int networkId, int playerId, bool isHost, string playerName, int characterId, int playerPlace, float[] playerColor)
            {
                this.networkId = networkId;
                this.playerId = playerId;
                this.isHost = isHost;
                this.playerName = playerName;
                this.characterId = characterId;
                this.playerPlace = playerPlace;
                this.playerColor = playerColor;
            }
        }
        
        [Serializable]
        public struct ObjectData
        {
            public int playerId;
            public int objectId;
            public int objectType;

            public ObjectPositionData positionData;

            public ObjectData(int playerId, int objectId, int objectType, float[] position, float[] rotation)
            {
                this.playerId = playerId;
                this.objectId = objectId;
                this.objectType = objectType;

                positionData = new ObjectPositionData(position, rotation);
            }
        }
        
        [Serializable]
        public struct ObjectPositionData
        {
            public uint lastPacketId;
            
            [MarshalAs(UnmanagedType.ByValArray,SizeConst = 3)]
            public float[] position;

            [MarshalAs(UnmanagedType.ByValArray,SizeConst = 4)]
            public float[] rotation;

            public ObjectPositionData(float[] position, float[] rotation)
            {
                this.position = position;
                this.rotation = rotation;
                lastPacketId = 0;
            }
        }

    }
}