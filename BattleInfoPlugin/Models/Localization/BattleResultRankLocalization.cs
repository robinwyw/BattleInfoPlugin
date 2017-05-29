using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Localization
{
    class BattleResultRankLocalization
    {
        public static string GetResource(BattleResultRank battleResultRank)
        {
            switch (battleResultRank)
            {
                case BattleResultRank.なし:
                    return Resources.Battle_Rank_None;
                case BattleResultRank.完全勝利S:
                    return Resources.Battle_Rank_Perfect;
                case BattleResultRank.勝利S:
                    return Resources.Battle_Rank_S;
                case BattleResultRank.勝利A:
                    return Resources.Battle_Rank_A;
                case BattleResultRank.戦術的勝利B:
                    return Resources.Battle_Rank_B;
                case BattleResultRank.戦術的敗北C:
                    return Resources.Battle_Rank_C;
                case BattleResultRank.敗北D:
                    return Resources.Battle_Rank_D;
                case BattleResultRank.敗北E:
                    return Resources.Battle_Rank_E;
            }
            return "";
        }
    }
}