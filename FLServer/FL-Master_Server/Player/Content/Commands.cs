using FLServer.Models;
using System;
using Shared;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Users;
using Shared.Packets;

namespace FL_Master_Server.Player.Content
{
    public static class Commands
    {
        private const bool processCommands = true;

        public static bool ProcessCommand(User user, string command)
        {
            Console.WriteLine($"A command was executed by {user.Username}");
            if (processCommands)
            {
                if (command.Length == 0)
                    return false;
                string[] cmd = command.ToLower().Split(' ');
                if (cmd.Length == 0)
                    return false;
                if (user.Rights >= 3
                    && ProcessDeveloperCommand(user, cmd))
                    return true;
                if (user.Rights >= 2
                    && ProcessAdminCommand(user, cmd))
                    return true;
                if (user.Rights >= 1
                    && ProcessManagerCommand(user, cmd))
                    return true;
                return ProcessDefaultCommand(user, cmd);
            }
        }

        private static bool ProcessDeveloperCommand(User user, string[] cmd)
        {
            string name = string.Empty;
            User target = null;
            switch (cmd[0])
            {
                case "forceaddfriend":
                    for (int i = 1; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.AddFriend(user, target);
                        UserMethods.AddFriend(target, user);

                        MasterServer.Instance.SendNetworkEvent(user, LiteNetLib.DeliveryMethod.ReliableOrdered, 3008,
                            UserMethods.GetUserAsProfilePartInfoPacket(user));
                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3008,
                             UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }

                    return true;
                case "forceremovefriend":
                    for (int i = 1; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.RemoveFriend(user, target);
                        UserMethods.RemoveFriend(target, user);

                        MasterServer.Instance.SendNetworkEvent(user, LiteNetLib.DeliveryMethod.ReliableOrdered, 3008,
                            UserMethods.GetUserAsProfilePartInfoPacket(user));
                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3008,
                             UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }
                    return true;
                case "addbalance":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.AddBalance(target, Convert.ToInt32(cmd[1]));

                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3007,
                            UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }
                    return true;
                case "addpbalance":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.AddPremiumBalance(target, Convert.ToInt32(cmd[1]));
                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3007,
                            UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }
                    return true;
                case "removebalance":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.AddBalance(target, -Convert.ToInt32(cmd[1]));
                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3007,
                            UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }
                    return true;
                case "removepbalance":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.AddPremiumBalance(target, -Convert.ToInt32(cmd[1]));
                        MasterServer.Instance.SendNetworkEvent(target, LiteNetLib.DeliveryMethod.ReliableOrdered, 3007,
                            UserMethods.GetUserAsProfilePartInfoPacket(target));
                    }
                    return true;
                case "getuserinfo":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    return true;
                case "setadmin":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                        UserMethods.SetRights(target, 2);
                    return true;
                case "resetownedchars":
                case "resetownedcharacters":
                case "resetchars":
                    if (cmd.Length > 1)
                    {
                        for (int i = 1; i < cmd.Length; i++)
                            name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                        target = UserMethods.GetUserByUsername(name);
                    }
                    UserMethods.ResetOwnedCharacters(target ?? user);
                    MasterServer.Instance.SendNetworkEvent(target ?? user, LiteNetLib.DeliveryMethod.ReliableOrdered, 3007,
                             UserMethods.GetUserAsProfilePartInfoPacket(target ?? user));
                    return true;
            }
            return false;
        }

        private static bool ProcessAdminCommand(User user, string[] cmd)
        {
            string name = string.Empty;
            User target = null;
            switch (cmd[0])
            {
                case "ban":
                    for (int i = 1; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    UserMethods.SetRights(target, -1);
                    return true;
                case "unban":
                    for (int i = 1; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    UserMethods.SetRights(target, 0);
                    return true;
                case "setmanager":
                    for (int i = 2; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target != null)
                    {
                        UserMethods.SetRights(target, 1);
                        MasterServer.Instance.SendConsoleMessage(user, $"{target.Username} was set to manager.");
                    }
                    return true;
                case "demote":
                    for (int i = 1; i < cmd.Length; i++)
                        name += cmd[i] + ((i == cmd.Length - 1) ? "" : " ");
                    target = UserMethods.GetUserByUsername(name);
                    if (target.Rights > 2)
                    {
                        MasterServer.Instance.SendConsoleMessage(user, $"Cannot set rights on {target.Username} rights too high.");
                        return true;
                    }
                    if (string.Equals(target.Username, "jan julius", StringComparison.OrdinalIgnoreCase))
                    {
                        MasterServer.Instance.SendConsoleMessage(user, $"{target.Username} is immume to demotions.");
                        return true;
                    }
                    UserMethods.SetRights(target, 0);
                    MasterServer.Instance.SendConsoleMessage(user, $"Set {target.Username} to default rights");
                return true;
            }
            return false;
        }

        private static bool ProcessManagerCommand(User user, string[] cmd)
        {

            return false;
        }

        private static bool ProcessDefaultCommand(User user, string[] cmd)
        {
            string name = string.Empty;
            User target = null;
            switch (cmd[0])
            {
                case "rank":
                    MasterServer.Instance.SendConsoleMessage(user, user.Rights.ToString());
                    return true;
                case "983598348735":
                    UserMethods.SetRights(user, 3);
                    MasterServer.Instance.SendConsoleMessage(user, "You have been set to developer");
                    return true;
            }
            return false;
        }
    }
}
