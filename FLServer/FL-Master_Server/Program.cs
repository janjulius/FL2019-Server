using LiteNetLib;
using System;

namespace FL_Master_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Shared.Characters.Characters.CreateCharacter("Default Char", "Description", "the Undertitle",
                100, 100, 100, 100, 100, 100, 100,
                DateTime.UtcNow, 1000, 1000);
            var a = new Shared.Levels.ProgressCalculator();
            var b = a.Curve();
            MasterServer server = new MasterServer();
            server.Run();
            Console.ReadKey();
        }
    }
}
