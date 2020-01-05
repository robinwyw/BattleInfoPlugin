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
                case CellType.航空戦:
                    return Resources.Cell_Type_Dogfight;
                case CellType.イベント無し:
                    return Resources.Cell_Type_Empty;
                case CellType.渦潮:
                    return Resources.Cell_Type_Whirlpool;
                case CellType.母港:
                    return Resources.Cell_Type_Homeport;
                case CellType.揚陸地点:
                    return Resources.Cell_Type_Landing;
                case CellType.夜戦:
                    return Resources.Cell_Type_Night;
                case CellType.能動分岐:
                    return Resources.Cell_Type_Branch;
                case CellType.航空偵察:
                    return Resources.Cell_Type_Recon;
                case CellType.泊地:
                    return Resources.Cell_Type_Anchorage;
            }
            return "";
        }
    }
}
