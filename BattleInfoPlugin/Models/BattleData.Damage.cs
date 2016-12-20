using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Raw;

namespace BattleInfoPlugin.Models
{
    public partial class BattleData
    {
        private void InjectionAirCombat(Api_Kouku airCombat)
        {
            if (airCombat == null) return;

            this.CalcAirCombatDamages(airCombat);

            this.InjectionAirCombatResults = airCombat.ToResult("噴-");
        }

        private void AirBaseAttack(Api_Air_Base_Attack[] attacks)
        {
            if (attacks == null) return;

            this.LandBaseAirCombatResults = attacks.ToResult();
            this.CalcDamages(attacks.GetDamages());
        }

        private void AirCombat(Api_Kouku airCombat, string prefix = "", bool airSupremacy = true)
        {
            if (airCombat == null) return;

            if (airSupremacy)
            {
                this.FriendAirSupremacy = airCombat.GetAirSupremacy();
            }

            this.CalcAirCombatDamages(airCombat);

            this.AirCombatResults = this.AirCombatResults.Concat(airCombat.ToResult(prefix)).ToArray();
        }

        private void Support(Api_Support_Info support)
        {
            if (support == null) return;

            var damages = support.GetDamages();
            this.CalcDamages(damages);
        }

        private void Shelling(Hougeki shelling, int friendFleetIndex = 1, int enemyFleetIndex = 1)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages(friendFleetIndex, enemyFleetIndex);
            this.CalcDamages(damages);
        }

        private void Shelling(Midnight_Hougeki shelling, int friendFleetIndex = 1, int enemyFleetIndex = 1)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages(friendFleetIndex, enemyFleetIndex);
            this.CalcDamages(damages);
        }

        private void Shelling(Enemy_Combined_Hougeki shelling)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages();
            this.CalcDamages(damages);
        }

        private void Torpedo(Raigeki torpedo, int friendFleetIndex = 1, int enemyFleetIndex = 1)
        {
            if (torpedo == null) return;

            var friendDamages = torpedo.GetFriendDamages(friendFleetIndex, enemyFleetIndex);
            var enemyDamages = torpedo.GetEnemyDamages(friendFleetIndex, enemyFleetIndex);

            this.CalcDamages(friendDamages);
            this.CalcDamages(enemyDamages);
        }


        private void CalcAirCombatDamages(Api_Kouku airCombat)
        {
            this.CalcDamages(airCombat.GetDamages(FleetType.Friend));
            this.CalcDamages(airCombat.GetDamages(FleetType.Enemy));
        }

        private void CalcDamages(IEnumerable<ShipDamage> damages)
        {
            if (damages == null) return;

            foreach (var damage in damages)
            {
                var source = this.GetShip(damage.Source);
                if (source != null)
                {
                    source.AttackDamage += damage.Damage;
                }

                var target = this.GetShip(damage.Target);
                if (target != null)
                {
                    target.ReceiveDamage(damage.Damage);

                    if (this.State == BattleState.InSortie)
                    {
                        target.CheckDamageControl();
                    }
                }
            }
        }

        private ShipData GetShip(int index)
        {
            if (index == 0) return null;

            return index > 0
                ? this.FriendFleet.GetShip(index)
                : this.EnemyFleet.GetShip(-index);
        }
    }
}
