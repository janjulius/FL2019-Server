using LiteNetLib;
using System;

namespace FL_Master_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            MasterServer server = new MasterServer();
            server.Run();
            Console.ReadKey();
        }

        

    }
}
