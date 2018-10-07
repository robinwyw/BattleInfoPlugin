using BattleInfoPlugin.Properties;
using System.Reflection;

namespace BattleInfoPlugin.Models.Localization
{
    class FormationLocalization
    {
        public static string GetResource(Formation airSupremacyResult)
        {
            switch (airSupremacyResult)
            {
                case Formation.なし:
                    return Resources.Formation_None;
                case Formation.不明:
                    return Resources.Formation_Unknown;
                case Formation.単縦陣:
                    return Resources.Formation_Line_Ahead;
                case Formation.複縦陣:
                    return Resources.Formation_Double_Line;
                case Formation.輪形陣:
                    return Resources.Formation_Diamond;
                case Formation.梯形陣:
                    return Resources.Formation_Echelon;
                case Formation.単横陣:
                    return Resources.Formation_Line_Abreast;
                case Formation.警戒陣:
                    return Resources.Formation_Vanguard;
                case Formation.対潜陣形:
                    return Resources.Formation_Combined_Anti_Sub;
                case Formation.前方陣形:
                    return Resources.Formation_Combined_Forward;
                case Formation.輪形陣形:
                    return Resources.Formation_Combined_Circle;
                case Formation.戦闘陣形:
                    return Resources.Formation_Combined_Line_Ahead;
            }
            return "";
        }
    }
}
