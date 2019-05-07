using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            LoginServer server = new LoginServer();
            server.Run();
            Console.ReadKey();
        }
    }
}
