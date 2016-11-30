using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin.Models
{
    public partial class BattleData
    {
        private enum BattleResultType
        {
            Normal,
            LdAirBattle
        }

        private BattleResultRank GetBattleResult()
        {
            if (!this.IsInBattle) return BattleResultRank.なし;

            var friendShips = this.FriendFleet.Fleets
                .SelectMany(f => f.Ships)
                .ToArray();

            var enemyShips = this.EnemyFleet.Fleets
                .SelectMany(f => f.Ships)
                .ToArray();

            var friendGuageRate = friendShips.GetHpLostPercent();
            var enemyGuageRate = enemyShips.GetHpLostPercent();

            this.FriendLostGauge = friendGuageRate;
            this.EnemyLostGauge = enemyGuageRate;

            var hpLostRatio = enemyGuageRate > 0 ? (friendGuageRate > 0 ? enemyGuageRate / friendGuageRate : 10) : 0;

            var friendLostCount = friendShips.GetLostCount();
            var friendCount = friendShips.Length;

            var enemyLostCount = enemyShips.GetLostCount();
            var enemyCount = enemyShips.Length;

            var isEnemyFlagshipSunk = enemyShips.First().NowHP <= 0;

            //SS or S
            if (enemyLostCount == enemyCount)
                if (friendGuageRate <= 0) return BattleResultRank.完全勝利S;
                else if (friendLostCount == 0) return BattleResultRank.勝利S;


            // A
            if (enemyLostCount >= Math.Max(enemyCount * 2 / 3, 1) && friendLostCount == 0) return BattleResultRank.勝利A;


            // B
            // 我方舰娘全部存活
            if (friendLostCount == 0)
            {
                // 敌旗舰击沉
                if (isEnemyFlagshipSunk) return BattleResultRank.戦術的勝利B;
                // 敌方损失比例为我方2.5倍以上
                if (hpLostRatio >= 2.5)
                    return BattleResultRank.戦術的勝利B;
            }
            // 我方存在击沉舰娘
            else
            {
                // 敌舰全击沉
                if (enemyLostCount == enemyCount) return BattleResultRank.戦術的勝利B;
                // 敌方HP损失比例为我方2.5倍以上
                if (hpLostRatio >= 2.5) return BattleResultRank.戦術的勝利B;
                // 敌方旗舰击沉 且 我方击沉数<敌方击沉数
                if (isEnemyFlagshipSunk && friendLostCount < enemyLostCount) return BattleResultRank.戦術的勝利B;
            }


            // C
            // 敌方旗舰击沉
            if (isEnemyFlagshipSunk) return BattleResultRank.戦術的敗北C;
            // 敌方旗舰未击沉 且 敌方HP损失比例:我方<2.5
            if (hpLostRatio >= 0.9 && hpLostRatio < 2.5) return BattleResultRank.戦術的敗北C;


            // E
            if (friendLostCount > 0 && friendCount - friendLostCount == 1) return BattleResultRank.敗北E;


            // D
            return BattleResultRank.敗北D;

        }

        private BattleResultRank GetBattleResult2()
        {
            this.FriendLostGauge = this.GetFriendHpLostPercent();
            this.EnemyLostGauge = this.GetEnemyHpLostPercent();

            var gauge = this.FriendLostGauge;
            if (gauge <= 0)
                return BattleResultRank.完全勝利S;
            if (gauge < 0.1)
                return BattleResultRank.勝利A;
            if (gauge < 0.2)
                return BattleResultRank.戦術的勝利B;
            if (gauge < 0.5)
                return BattleResultRank.戦術的敗北C;
            if (gauge < 0.8)
                return BattleResultRank.敗北D;

            return BattleResultRank.敗北E;
        }

        
    }

    internal static class BattleResultHelper
    {
        public static double GetFriendHpLostPercent(this BattleData battleData)
        {
            return battleData.FriendFleet.Fleets
                .SelectMany(f => f.Ships)
                .GetHpLostPercent();
        }

        public static double GetEnemyHpLostPercent(this BattleData battleData)
        {
            return battleData.EnemyFleet.Fleets
                .SelectMany(f => f.Ships)
                .GetHpLostPercent();
        }

        public static double GetHpLostPercent(this IEnumerable<ShipData> data)
        {
            var ships = data.Where(s => !s.IsInEvacuationOrTow()).ToArray();
            var totalOriginalHp = ships.Sum(s => s.OriginalHP);
            var totalNowHp = ships.Sum(s => s.NowHP);

            return 1.0 - (double)totalNowHp / totalOriginalHp;
        }

        public static int GetLostCount(this ShipData[] data) => data.Count(s => s.NowHP <= 0);

        public static bool IsInEvacuationOrTow(this ShipData data) =>
            data.Situation.HasFlag(ShipSituation.Evacuation) || data.Situation.HasFlag(ShipSituation.Tow);
    }
}
