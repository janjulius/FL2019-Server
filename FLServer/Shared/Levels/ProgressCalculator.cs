using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Levels
{
    public class ProgressCalculator
    {
        int minlevel = 1;
        int maxlevel = 100;
        double increase = 300.0;
        double difference = 1.5;
        double divider = 4;
        double div = 8.0f;

        public Dictionary<int, double> Curve()
        {
            Dictionary<int, double> lvlExpValues = new Dictionary<int, double>(); //lvl:exp

            double points = 0;
            double output = 0;

            for (int lvl = minlevel; lvl <= maxlevel; lvl++)
            {
                points += Math.Floor(lvl + increase * Math.Pow(difference, lvl / div));
                output = (int)Math.Floor(points / divider);
                if (lvl >= minlevel)

                    output = Math.Floor(points / divider);
                lvlExpValues.Add(lvl, output);
            }
            return lvlExpValues;
        }

        public double getExperienceByLevel(int level)
        {
            double points = 0;
            double output = 0;

            for (int lvl = minlevel; lvl <= level; lvl++)
            {
                points += Math.Floor(lvl + increase
                        * Math.Pow(difference, lvl / div));
                if (lvl >= level)
                {
                    return output;
                }
                output = (int)Math.Floor(points / divider);
            }
            return 0;
        }

        public int GetLevelByExperience(double experience)
        {
            int rlvl = -1;
            double points = 0;
            double output = 0;

            for (int lvl = minlevel; lvl <= maxlevel; lvl++)
            {
                points += Math.Floor(lvl + increase * Math.Pow(difference, lvl / div));
                output = (int)Math.Floor(points / divider);
                if (lvl >= minlevel)
                    output = Math.Floor(points / divider);
                if (experience >= output)
                    return lvl;
            }

            return rlvl;
        }
    }
}
