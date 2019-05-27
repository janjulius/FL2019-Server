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

        public static void UpdateVersion(string versionNumber)
        {
            using (var ctx = new FLDBContext())
            {
                string n = string.Empty;
                try
                {
                    n = ctx.ServerVersion.First().VersionNr;
                }
                catch { }
                if (n == versionNumber)
                {
                    return;
                }
                ctx.ServerVersion.First().VersionNr = versionNumber;
                ctx.SaveChanges();
            }
        }

        public static void SetVersion(string versionNumber)
        {
            using (var ctx = new FLDBContext())
            {
                string n = string.Empty;
                ServerVersion s = new ServerVersion() { VersionId = new Guid().ToString(), VersionNr = versionNumber };
                try
                {
                    n = ctx.ServerVersion.First().VersionNr;
                }
                catch {

                    ctx.Add(s);
                }
                ctx.SaveChanges();
            }
        }
    }
}
