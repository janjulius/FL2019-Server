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

            if (Constants.UpdateDatabase)
            {
                Shared.General.General.UpdateVersion(Constants.ServerVersion);
            }
            if (Constants.InitializeDatabase)
            {
                Shared.General.General.SetVersion(Constants.ServerVersion);
            }

            MasterServer server = new MasterServer();
            server.Run();
            Console.ReadKey();
        }
    }
}
