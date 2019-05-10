using FLServer.Models;
using Shared.Users;
using System.Linq;

namespace Shared.Authentication
{
    public static class UserAuth
    {
        public static bool AuthenticateUser(string name, string pass)
        {
            return VerifyPassword(name, pass);
        }

        public static bool VerifyPassword(string name, string hashedPassword)
        {
            using (var ctx = new FLDBContext())
            {
                if (UserMethods.UserExists(name))
                {
                    return ctx.User.Where(u => u.Username == name).First().Password == hashedPassword;
                }
            }
            return false;
        }
    }
}