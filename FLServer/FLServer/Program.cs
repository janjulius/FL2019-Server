using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.WriteLine(server.AddNewUser("Wesket").ToString());
            Console.WriteLine(server.GetUserLvl(99));
            Console.ReadKey();
        }
    }
}
