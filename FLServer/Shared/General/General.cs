using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.General
{
    public static class General
    {
        /// <summary>
        /// Returns the current version of the game
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            using (var ctx = new FLDBContext())
            {
                return ctx.ServerVersion.First().VersionNr;
            }
        }

        public static void SetNewVersion(string versionNumber)
        {
            using (var ctx = new FLDBContext())
            {
                var n = ctx.ServerVersion.First().VersionNr;
                if (n == versionNumber)
                {
                    return;
                }
                ctx.ServerVersion.First().VersionNr = versionNumber;
                ctx.SaveChanges();
            }
        }
    }
}
