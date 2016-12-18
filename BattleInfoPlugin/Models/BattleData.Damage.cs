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

            this.CalDamage(airCombat);

            this.InjectionAirCombatResults = airCombat.ToResult("噴-");
        }

        private void AirBaseAttack(Api_Air_Base_Attack[] attacks)
        {
            if (attacks == null) return;

            this.LandBaseAirCombatResults = attacks.ToResult();
            foreach (var fleet in this.EnemyFleet.Fleets)
            {
                fleet.CalcDamages(attacks.GetDamages(fleet.Index));
            }
        }

        private void AirCombat(Api_Kouku airCombat, string prefix = "", bool airSupremacy = true)
        {
            if (airCombat == null) return;

            if (airSupremacy)
            {
                this.FriendAirSupremacy = airCombat.GetAirSupremacy();
            }

            this.CalDamage(airCombat);

            this.AirCombatResults = this.AirCombatResults.Concat(airCombat.ToResult(prefix)).ToArray();
        }

        private void CalDamage(Api_Kouku airCombat)
        {
            foreach (var fleet in this.FriendFleet.Fleets)
            {
                fleet.CalcDamages(airCombat.GetDamages(FleetType.Friend, fleet.Index));
            }
            foreach (var fleet in this.EnemyFleet.Fleets)
            {
                fleet.CalcDamages(airCombat.GetDamages(FleetType.Enemy, fleet.Index));
            }
        }

        private void Support(Api_Support_Info support)
        {
            if (support == null) return;

            if (support.api_support_airatack != null)
            {
                foreach (var fleet in this.EnemyFleet.Fleets)
                {
                    fleet.CalcDamages(support.api_support_airatack.GetDamages(FleetType.Enemy, fleet.Index));
                }
            }
            else if (support.api_support_hourai?.api_damage != null)
            {
                var damages = support.api_support_hourai.api_damage.GetCombinedDamages();
                foreach (var fleet in this.EnemyFleet.Fleets)
                {
                    fleet.CalcDamages(damages[fleet.Index]);
                }
            }
        }

        private void Shelling(Hougeki shelling, int friendFleetIndex = 0, int enemyFleetIndex = 0)
        {
            if (shelling == null) return;

            var friendDamage = shelling.GetFriendDamages();
            var enemyDamage = shelling.GetEnemyDamages();
            this.FriendFleet.Fleets[friendFleetIndex].CalcDamages(friendDamage);
            this.EnemyFleet.Fleets[enemyFleetIndex].CalcDamages(enemyDamage);
        }

        private void Shelling(Midnight_Hougeki shelling, int friendFleetIndex = 0, int enemyFleetIndex = 0)
        {
            if (shelling == null) return;

            var friendDamage = shelling.GetFriendDamages();
            var enemyDamage = shelling.GetEnemyFirstFleetDamages();
            this.FriendFleet.Fleets[friendFleetIndex].CalcDamages(friendDamage);
            this.EnemyFleet.Fleets[enemyFleetIndex].CalcDamages(enemyDamage);
        }

        private void Shelling(Enemy_Combined_Hougeki shelling)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages();

            foreach (var damage in damages[0])
            {
                this.FriendFleet.Fleets[damage.Key].CalcDamages(damage.Value.ToArray());
            }

            foreach (var damage in damages[1])
            {
                this.EnemyFleet.Fleets[damage.Key].CalcDamages(damage.Value.ToArray());
            }
        }

        private void Torpedo(Raigeki torpedo, int friendFleetIndex = 0, int enemyFleetIndex = 0)
        {
            if (torpedo == null) return;

            var friendDamage = torpedo.GetFriendDamages();
            var enemyDamage = torpedo.GetEnemyDamages();
            this.FriendFleet.Fleets[friendFleetIndex].CalcDamages(friendDamage);
            this.EnemyFleet.Fleets[enemyFleetIndex].CalcDamages(enemyDamage);
        }

        private void TorpedoCombined(Raigeki torpedo)
        {
            if (torpedo == null) return;

            var friendDamages = torpedo.GetFriendDamagesCombined();
            var enemyDamages = torpedo.GetEnemyDamagesCombined();
            for (var i = 0; i < friendDamages.Length; i++)
            {
                this.FriendFleet.Fleets[i].CalcDamages(friendDamages[i]);
            }

            for (var i = 0; i < enemyDamages.Length; i++)
            {
                this.EnemyFleet.Fleets[i].CalcDamages(enemyDamages[i]);
            }
        }
    }
}
