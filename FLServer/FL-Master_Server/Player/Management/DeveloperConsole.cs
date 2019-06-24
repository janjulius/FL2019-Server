using FL_Master_Server.Net;
using FLServer.Models;
using LiteNetLib;
using System;
using System.Collections.Generic;
using System.Text;

namespace FL_Master_Server.Player.Management
{
    public static class DeveloperConsole
    {

        public static void SendConsoleMessage(User target, string message)
        {
            NetEvent.SendNetworkEventString(Util.GetNetworkUserFromUser(target), DeliveryMethod.ReliableOrdered, 20000, message);
        }
    }
}
