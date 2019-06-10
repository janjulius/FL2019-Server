using LiteNetLib;
using System;
using System.Text;

namespace FL_Master_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilder text = new StringBuilder();

            foreach(var arg in args)
            {
                text.Append(arg + " ");
            }

            Console.WriteLine($"Running application with arguements: {text}");

            if (args.Length > 0)
                bool.TryParse(args[0], out Settings.UIActive);

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
