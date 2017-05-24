using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Localization
{
    class BattleSituationLocalization
    {
        public static string GetResource(BattleSituation airSupremacyResult)
        {
            switch (airSupremacyResult)
            {
                case BattleSituation.なし:
                    return Resources.Battle_Situation_None;
                case BattleSituation.同航戦: //Parallel Engagement
                    return Resources.Battle_Situation_Parallel;
                case BattleSituation.反航戦: //Head-on Engagement
                    return Resources.Battle_Situation_HeadOn;
                case BattleSituation.T字有利: //Crossing the T (Advantage) aka Green T
                    return Resources.Battle_Situation_TAdv;
                case BattleSituation.T字不利: //Crossing the T (Disadvantage) aka Red T
                    return Resources.Battle_Situation_TDis;
            }
            return "";
        }
    }
}
