using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.WriteLine(server.AddNewUser("Jan", "test").ToString());
            Console.WriteLine(server.AddNewUser("Wesket", "test").ToString());
            Console.WriteLine(server.GetUserLvl(99));
            Console.WriteLine(server.AddNewUser("Jan Julius", "test").ToString());
            Console.WriteLine(server.AddNewUser("Thosmas", "test").ToString());
            Console.WriteLine(server.AddFriend("Thosmas", "Wesket").ToString());
            Console.WriteLine(server.AddNewUser("xD", "xD").ToString());
            Console.WriteLine(server.AddFriend("Jan", "Thosmas").ToString());
            Console.WriteLine(server.AddFriend("Wesket", "Jan").ToString());
            Console.WriteLine(server.AddFriend("Jan", "Wesket").ToString());
            Console.WriteLine(server.GetFriends("Jan").ToString());
            Console.ReadKey();
        }
    }
}
