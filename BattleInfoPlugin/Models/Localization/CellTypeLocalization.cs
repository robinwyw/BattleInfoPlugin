using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Localization
{
    class CellTypeLocalization
    {
        public static string GetResource(CellType airSupremacyResult)
        {
            switch (airSupremacyResult)
            {
                case CellType.None:
                    return Resources.Cell_Type_None;
                case CellType.戦闘:
                    return Resources.Cell_Type_Battle;
                case CellType.空襲戦:
                    return Resources.Cell_Type_Air_Raid;
                case CellType.気のせい:
                    return Resources.Cell_Type_Imagination;
                case CellType.ボス:
                    return Resources.Cell_Type_Boss;
            }
            return "";
        }
    }
}
