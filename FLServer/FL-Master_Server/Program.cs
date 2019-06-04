using LiteNetLib;
using System;

namespace FL_Master_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            if (Constants.UpdateDatabase)
            {
                Shared.General.General.UpdateVersion(Constants.ServerVersion);
            }
            if (Constants.InitializeDatabase)
            {
                Shared.General.General.SetVersion(Constants.ServerVersion);
            }
            //Shared.Characters.Characters.Truncate();
            MasterServer.Instance.Run();
            Console.ReadKey();
        }
    }
}
