using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Users.Content
{
    public static class Commands
    {
        public const bool processCommands = true;

        public static bool ProcessCommand(User user, string command)
        {
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

        public static bool ProcessDeveloperCommand(User user, string[] cmd)
        {
            switch (cmd[0])
            {
                case "addfriend":
                    if(cmd.Length == 2)
                    {

                    }
                    break;
            }
            return true;
        }

        public static bool ProcessAdminCommand(User user, string[] cmd)
        {

            return true;
        }

        public static bool ProcessManagerCommand(User user, string[] cmd)
        {

            return true;
        }

        public static bool ProcessDefaultCommand(User user, string[] cmd)
        {

            return true;
        }
    }
}
