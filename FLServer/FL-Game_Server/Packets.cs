using System;
using System.Runtime.InteropServices;

namespace FL_Game_Server.Packets
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

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
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

        public ObjectData(int playerId, int objectId, int objectType, Position pos, Rotation rot)
        {
            this.playerId = playerId;
            this.objectId = objectId;
            this.objectType = objectType;

            positionData = new ObjectPositionData(pos, rot);
        }
    }

    [Serializable]
    public struct ObjectPositionData
    {
        public uint lastPacketId;

        public Position position;
        public Rotation rotation;

        public ObjectPositionData(Position position, Rotation rotation)
        {
            this.position = position;
            this.rotation = rotation;
            lastPacketId = 0;
        }
    }

    [Serializable]
    public struct Position
    {
        public float x;
        public float y;
        public float z;

        public Position(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    [Serializable]
    public struct Rotation
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Rotation(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}