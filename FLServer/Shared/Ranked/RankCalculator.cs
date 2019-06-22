using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Ranked
{
    public class RankCalculator
    {
        internal const double eloBorder = 49; //50 in practice
        internal const double I = 2500;
        internal const double II = 2250;
        internal const double III = 2000;
        internal const double IV = 1500;
        internal const double V = 1000;
        internal const double VI = 0;
        internal const double baseELO = 1000;
        internal const double baseChange = 10;


        internal const double OpponentratingMultiplier = 0.01;

        public static double GetBaseELO()
        {
            return baseELO;
        }

        public static string GetNewRank(int CurrentELO, string PreviousRank)
        {
            string[] rankStrings = new string[6] { "I", "II", "III", "IV", "V", "VI" };
            int[] rankInts = new int[6] { 2500, 2250, 2000, 1500, 1000, 0 };

            for (int i = 0; i < 6; i++)
            {
                if (CurrentELO > (rankInts[i] + eloBorder))
                {
                    return rankStrings[i];
                }
                else if (CurrentELO < (rankInts[i] - eloBorder)) { }
                else
                {
                    return PreviousRank;
                }
            }
            return "Something went wrong while calculating rank";
        }

        public static double GetNewELO(double CurrentELO, double OpponentELO, bool win)
        {
            double newELO = 0;

            if (win == true)
            {
                newELO = (CurrentELO + (baseChange + (OpponentELO * OpponentratingMultiplier) * (OpponentELO / CurrentELO)));
            }
            else if (win == false)
            {
                newELO = (CurrentELO - (baseChange + (OpponentELO * OpponentratingMultiplier)) * (CurrentELO / OpponentELO));
            }

            return newELO;
        }
    }
}