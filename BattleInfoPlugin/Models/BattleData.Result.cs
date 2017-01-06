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

            this.FriendFleetStatus = this.FriendFleet.GetStatus();
            this.EnemyFleetStatus = this.EnemyFleet.GetStatus();

            var friendGuageRate = this.FriendFleetStatus.LostGauge;
            var enemyGuageRate = this.EnemyFleetStatus.LostGauge;

            var hpLostRatio = enemyGuageRate > 0 ? (friendGuageRate > 0 ? enemyGuageRate / friendGuageRate : 10) : 0;

            var friendLostCount = this.FriendFleetStatus.LostCount;
            var friendCount = this.FriendFleetStatus.ShipCount;

            var enemyLostCount = this.EnemyFleetStatus.LostCount;
            var enemyCount = this.EnemyFleetStatus.ShipCount;

            var isEnemyFlagshipSunk = this.EnemyFleet.GetShip(1).NowHP <= 0;


            //SS or S
            if (this.EnemyFleetStatus.LostCount == this.EnemyFleetStatus.ShipCount)
                if (this.FriendFleetStatus.LostGauge <= 0) return BattleResultRank.完全勝利S;
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
            this.FriendFleetStatus = this.FriendFleet.GetStatus();
            this.EnemyFleetStatus = this.EnemyFleet.GetStatus();

            var gauge = this.FriendFleetStatus.LostGauge;
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
}
