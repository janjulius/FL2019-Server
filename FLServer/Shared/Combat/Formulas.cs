using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Combat
{
    public class Formulas
    {
        internal const int BaseDefence = 20;

        internal const float BasePercentageMultiplier = 0.5f;

        internal const float BaseVelocity = 10;
        internal const float VelocityNewDamageDivider = 20;
        internal const float VelocityWeightDivider = 100;
        internal const float VelocityMultiplier = 1;
        internal const float KnockBackPercentageDivider = 2.5f;

        internal const float DamageMultiplier = 1;

        internal const int PercentageDecimals = 2;

        public static float GetRegularDamage(int aDamage, 
            int dDefence, 
            float dPercentage)
        {
            return regularDamageFormula(aDamage, dDefence, dPercentage);
        }

        public static float GetRegularDamage(int aDamage, 
            float aDamageMultiplier, 
            int dDefence, 
            float dDefenceMultiplier,
            float dPercentage)
        {
            float a, d;
            a = aDamage * aDamageMultiplier;
            d = dDefence * dDefenceMultiplier;
            return regularDamageFormula(a, d, dPercentage);
        }

        public static float CalculateKnockBackVelocity(int dWeight, float dPercentage, float newDamage)
        {
            return BaseVelocity + ((
                (newDamage / VelocityNewDamageDivider)
                *
                (dPercentage / KnockBackPercentageDivider)
                + VelocityMultiplier)
                /
                (dWeight / VelocityWeightDivider));

        }

        private static float regularDamageFormula(float aDmg, 
            float dDef, 
            float dPercentage)
        {
            float dmgMultiplier = dDef >= 0 ? BaseDefence / (BaseDefence + dDef) : BaseDefence / (BaseDefence + dDef);
            return DamageToPercentage((float)aDmg* dmgMultiplier, dPercentage);
        }

        private static float DamageToPercentage(float flatDamage, float percentage)
        {
            return 
                (float)Math.Round((flatDamage / 
                ((percentage >= 26) ? 
                percentage * (float)Math.Pow(BasePercentageMultiplier, percentage/ 100) : 
                22.5f) 
                * 10), 
                PercentageDecimals);
        }



       
    }
}
