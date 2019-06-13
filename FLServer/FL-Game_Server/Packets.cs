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

        public ColorPacket playerColor;

        public PlayerGameInfo gameInfo;

        public PlayerStats playerStats;

        public PlayerInfo(int networkId, int playerId, bool isHost, string playerName, int characterId, int playerPlace, ColorPacket playerColor, PlayerGameInfo gameInfo, PlayerStats playerStats)
        {
            this.networkId = networkId;
            this.playerId = playerId;
            this.isHost = isHost;
            this.playerName = playerName;
            this.characterId = characterId;
            this.playerPlace = playerPlace;
            this.playerColor = playerColor;
            this.gameInfo = gameInfo;
            this.playerStats = playerStats;
        }
    }

    [Serializable]
    public struct PlayerGameInfo
    {
        public float damage;
        public float health;
        public byte lives;
        public byte spawnPlace;

        public PlayerGameInfo(float damage, float health, byte lives, byte spawnPlace)
        {
            this.damage = damage;
            this.health = health;
            this.lives = lives;
            this.spawnPlace = spawnPlace;
        }
    }
    
    [Serializable]
    public struct PlayerStats
    {
        public int kills;
        public int deaths;
        public int ultsUsed;
        public float damageDone;
        public float damageTaken;
        public float damageBlocked;
        public float highestDamageSurvived;
        public float damageHealed;
        public float damageMissed;

        public PlayerStats(int kills, int deaths, int ultsUsed, float damageDone, float damageTaken, float damageBlocked, float highestDamageSurvived, float damageHealed, float damageMissed)
        {
            this.kills = kills;
            this.deaths = deaths;
            this.ultsUsed = ultsUsed;
            this.damageDone = damageDone;
            this.damageTaken = damageTaken;
            this.damageBlocked = damageBlocked;
            this.highestDamageSurvived = highestDamageSurvived;
            this.damageHealed = damageHealed;
            this.damageMissed = damageMissed;
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

    [Serializable]
    public struct ColorPacket
    {
        public float r;
        public float g;
        public float b;

        public ColorPacket(float r, float g, float b)
        {
            this.r = r;
            this.g = g;
            this.b = b;
        }
    }
    
    [Serializable]
    public struct Damage
    {
        public int damageDealerId;
        public int damageTakerId;
        public float damage;
        public float directionX;
        public float directionY;
        public byte damageType;
        public long timeStamp;

        public Damage(int damageDealerId, int damageTakerId, float damage, float directionX, float directionY, byte damageType)
        {
            this.damageDealerId = damageDealerId;
            this.damageTakerId = damageTakerId;
            this.damage = damage;
            this.directionX = directionX;
            this.directionY = directionY;
            this.damageType = damageType;
            timeStamp = DateTime.UtcNow.ToBinary();
        }
    }
}