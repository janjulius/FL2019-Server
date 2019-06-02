using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace FL_Master_Server
{
    class Constants
    {
        internal static int Port = 9052;
        internal static int MaxConnections = 1000;
        internal static int StartGameServerPort = 10000;
        internal static string ConnectionKey = "masterserver";

        internal const string CantFindProfile = "User does not exist";

        internal const string ServerVersion = "0.2";
        internal const bool InitializeDatabase = true;
        internal const bool UpdateDatabase = false;

        internal const bool ValidateNetworkUsers = true;
    }
}