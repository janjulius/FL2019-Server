﻿using System;

namespace FLServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.Run();
            Console.ReadKey();
        }
    }
}
