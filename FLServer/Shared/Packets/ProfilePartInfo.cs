using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Packets
{
        public class ProfilePartInfo : Packet
        {
            public string UserName { get; set; }
            public int Currency { get; set; }
            public int PremiumCurrency { get; set; }
            public int Avatar { get; set; }
            public int Level { get; set; }
            public int Exp { get; set; }

            public ProfilePartInfo() { }

            public ProfilePartInfo(string userName, int currency, int premiumCurrency, int avatar, int level, int exp)
            {
                UserName = userName;
                Currency = currency;
                PremiumCurrency = premiumCurrency;
                Avatar = avatar;
                Level = level;
                Exp = exp;
            }
        }
}
