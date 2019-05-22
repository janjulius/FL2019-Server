namespace FL_Master_Server
{
    public class GameServerInfo
    {
        public string serverName;
        public int port;
        public string masterKey;
        public byte roomType;
        public byte totalPlayers = 0;
        public byte maxPlayers;

        public GameServerInfo(string serverName, int port, string masterKey, byte roomType, byte maxPlayers)
        {
            this.serverName = serverName;
            this.port = port;
            this.masterKey = masterKey;
            this.roomType = roomType;
            this.maxPlayers = maxPlayers;
        }
    }
}