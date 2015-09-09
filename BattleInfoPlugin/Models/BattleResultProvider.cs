using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin.Models
{
    internal static class BattleResultProvider
    {
        internal static BattleResult GetBattleResult(this BattleData battleData)
        {
            if (!battleData.IsInBattle) return BattleResult.なし;
            
            var friendShips = battleData.FirstFleet.Ships.Where(s => !s.IsInEvacuationOrTow())
                .Concat(battleData.SecondFleet?.Ships.Where(s => !s.IsInEvacuationOrTow()) ?? new ShipData[0])
                .ToArray();

            var enemyShips = battleData.Enemies.Ships.ToArray();

            var friendGuageRate = friendShips.GetHpLostPersent();
            var enemyGuageRate = enemyShips.GetHpLostPersent();
            var hpLostRatio = enemyGuageRate > 0 ? (friendGuageRate > 0 ? enemyGuageRate / friendGuageRate : 10) : 0;

            var friendLostCount = friendShips.GetLostCount();
            var friendCount = friendShips.Length;

            var enemyLostCount = enemyShips.GetLostCount();
            var enemyCount = enemyShips.Length;

            var isEnemyFlagshipSunk = enemyShips.First().NowHP <= 0;

            //SS or S
            if (enemyLostCount == enemyCount)
                if (friendGuageRate <= 0) return BattleResult.完全勝利S;
                else if (friendLostCount == 0) return BattleResult.勝利S;
            

            // A
            if (enemyLostCount >= Math.Max(enemyCount * 2 / 3, 1) && friendLostCount == 0) return BattleResult.勝利A;


            // B
            // 我方舰娘全部存活
            if (friendLostCount == 0)
            {
                // 敌旗舰击沉
                if (isEnemyFlagshipSunk) return BattleResult.戦術的勝利B;
                // 敌方损失比例为我方2.5倍以上
                if (hpLostRatio >= 2.5)
                    return BattleResult.戦術的勝利B;
            }
            // 我方存在击沉舰娘
            else
            {
                // 敌舰全击沉
                if (enemyLostCount == enemyCount) return BattleResult.戦術的勝利B;
                // 敌方HP损失比例为我方2.5倍以上
                if (hpLostRatio >= 2.5) return BattleResult.戦術的勝利B;
                // 敌方旗舰击沉 且 我方击沉数<敌方击沉数
                if (isEnemyFlagshipSunk && friendLostCount < enemyLostCount) return BattleResult.戦術的勝利B;
            }


            // C
            // 敌方旗舰击沉
            if (isEnemyFlagshipSunk) return BattleResult.戦術的敗北C;
            // 敌方旗舰未击沉 且 敌方HP损失比例:我方<2.5
            if (hpLostRatio >= 0.9 && hpLostRatio < 2.5) return BattleResult.戦術的敗北C;


            // E
            if (friendLostCount > 0 && friendCount - friendLostCount == 1) return BattleResult.敗北E;


            // D
            return BattleResult.敗北D;

        }

        private static double GetHpLostPersent(this ShipData[] data)
        {
            var totalOriginalHP = data.Sum(s => s.OriginalHP);
            var totalNowHP = data.Sum(s => s.NowHP);

            return 1.0 - (double)totalNowHP / totalOriginalHP;
        }

        private static int GetLostCount(this ShipData[] data) => data.Count(s => s.NowHP <= 0);

        private static bool IsInEvacuationOrTow(this ShipData data) =>
            data.Situation.HasFlag(ShipSituation.Evacuation) || data.Situation.HasFlag(ShipSituation.Tow);
    }
}
