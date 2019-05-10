using LiteNetLib;
using System;

namespace FL_Master_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new Shared.Levels.ProgressCalculator();
            var b = a.Curve();
            MasterServer server = new MasterServer();
            server.Run();
            Console.ReadKey();
        }

        

    }
}
