using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Users
{
    public static class UserMethods
    {
        public static bool UserExists(string name)
        {
            using (var ctx = new FLDBContext())
            {
                var usr = ctx.User.Where(user => user.Username == name);
        
                if (usr.Any())
                    return true;
            }
            return false;
        }
        
        public static void UpdateLastLogin(string username)
        {
            using (var ctx = new FLDBContext())
            {
                ctx.User.Where(u => u.Username == username).First().LastOnline = DateTime.UtcNow;
            }
        }

        public static int GetUserBalance(string username)
        {
            return GetUserByUsername(username).Balance;
        }

        public static int GetUserPremiumBalance(string username)
        {
            return GetUserByUsername(username).PremiumBalance;
        }

        private static User GetUserByUsername(string username)
        {
            using (var ctx = new FLDBContext())
            {
                return ctx.User.Where(u => u.Username == username).First();
            }
        }

    }
}
