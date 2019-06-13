using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Ranked
{
    public class RankCalculator
    {
        public double newElo;
        private double eloBorder = 49; //50 in practice
        private const int amountOfRanks = 6;
        private double baseELO = 1000;
        private double baseChange = 10;
        private bool promoted, demoted, aboveCurrentRank, belowCurrentRank = false;
        private string[] rankStrings = new string[amountOfRanks] { "I", "II", "III", "IV", "V", "VI" };
        private int[] rankInts = new int[amountOfRanks] { 2500, 2250, 2000, 1500, 1000, 0 };
        private double OpponentratingMultiplier = 0.01;
        
        public RankCalculator()
        {
            newElo = 0;
        }
        public double GetBaseELO()
        {
            return baseELO;
        }

        public string GetCurrentRank(double CurrentELO)
        {
            for(int i = 0; i < rankInts.Length; i++)
            {
                if(CurrentELO > rankInts[i])
                {
                    return rankStrings[i];
                }
            }

            return "";
        }

        public string GetCurrentAbsoluteRank(double CurrentELO)
        {
            if(aboveCurrentRank)
            {
                return GetCurrentRank(CurrentELO - 50);
            }
            else if(belowCurrentRank)
            {
                return GetCurrentRank(CurrentELO + 50);
            }
            else
            {
                return GetCurrentRank(CurrentELO);
            }
        }

        public double GetNewELO(double CurrentELO, double OpponentELO, bool win)
        {
            if (win)
            {
                newElo = CurrentELO + (baseChange + (OpponentELO * OpponentratingMultiplier) * (OpponentELO / CurrentELO));
                if (GetCurrentRank(newElo + eloBorder) != GetCurrentRank(CurrentELO))
                {
                    promoted = true;
                }
                else if(GetCurrentRank(newElo) != GetCurrentRank(CurrentELO))
                {
                    aboveCurrentRank = true;
                }
            }
            else
            {
                newElo = CurrentELO - (baseChange + (OpponentELO * OpponentratingMultiplier)) * (CurrentELO / OpponentELO);
                if(GetCurrentRank(newElo - eloBorder) != GetCurrentRank(CurrentELO))
                {
                    demoted = true;
                }
                else if(GetCurrentRank(newElo) != GetCurrentRank(CurrentELO))
                {
                    belowCurrentRank = true;
                }
            }

            return newElo;
        }

    }
}
