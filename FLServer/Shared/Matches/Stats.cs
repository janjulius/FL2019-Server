using FLServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Matches
{
    public static class Stats
    {

        public static FLServer.Models.Stats Create(
        int 
            damageDealt 
        ,int highestPercentage 
        ,int kills 
        ,int deaths )
        {
            using (FLDBContext ctx = new FLDBContext())
            {
                FLServer.Models.Stats s = new FLServer.Models.Stats()
                {
                    DamageDealt = damageDealt,
                    HighestPercentage = highestPercentage,
                    Kills = kills,
                    Deaths = deaths
                };
                ctx.Stats.Add(s);
                ctx.SaveChanges();
                return s;
            }
        }
    }
}
