using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.WriteLine(server.AddNewUser("Jan").ToString());
            Console.WriteLine(server.AddNewUser("Wesket").ToString());
            Console.WriteLine(server.GetUserLvl(99));
            Console.WriteLine(server.AddNewUser("Jan Julius").ToString());
            Console.WriteLine(server.AddNewUser("Thosmas").ToString());
            Console.WriteLine(server.AddFriend("Thosmas", "Wesket").ToString());
            Console.WriteLine(server.AddFriend("Jan", "Thosmas").ToString());
            Console.WriteLine(server.AddFriend("Wesket", "Jan").ToString());
            Console.WriteLine(server.AddFriend("Jan", "Wesket").ToString());
            Console.ReadKey();
        }
    }
}
