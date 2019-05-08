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

        public static bool VerifyPassword(string name, string password)
        {
            using (var ctx = new FLDBContext())
            {
                if (UserMethods.UserExists(name))
                {
                    return ctx.User.Where(u => u.Username == name).First().Password == password;
                }
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

        public static IEnumerable<User> GetFriends(string name)
        {
            IEnumerable<User> users;

            using (var ctx = new FLDBContext())
            {
                var user = ctx.User.Where(u => u.Username == name).First();

                var res = ctx.UserFriend.Where(u => u.UserId == user.UserId).AsEnumerable();

                
            }
        }

    }
}
