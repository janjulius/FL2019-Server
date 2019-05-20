using System;

namespace FL_Game_Server
{
    class Program
    {

        static void Main(string[] args)
        {
            GameServer server = new GameServer();

            server.Run(args);
        }
    }
}