﻿using System;
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
            foreach (var attack in attacks)
            {
                this.CalcAirCombatDamages(attack);
            }
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

        private void Support(Api_Support_Info support, int supportType)
        {
            if (support == null) return;

            var damages = support.GetDamages(supportType);
            this.CalcDamages(damages, true);
        }

        private void Shelling(Hougeki shelling)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages();
            this.CalcDamages(damages);
        }

        private void Shelling(Midnight_Hougeki shelling, bool friendlySupport = false)
        {
            if (shelling == null) return;
            if (shelling.api_at_eflag == null) return;

            var damages = shelling.GetDamages(friendlySupport);
            this.CalcDamages(damages);
        }

        private void Shelling(Enemy_Combined_Hougeki shelling)
        {
            if (shelling == null) return;

            var damages = shelling.GetDamages();
            this.CalcDamages(damages);
        }

        private void Torpedo(Raigeki torpedo)
        {
            if (torpedo == null) return;

            var friendDamages = torpedo.GetFriendDamages();
            var enemyDamages = torpedo.GetEnemyDamages();

            this.CalcDamages(friendDamages, true);
            this.CalcDamages(enemyDamages, true);
        }

        private void TorpedoOpen(Raigeki torpedo)
        {
            if (torpedo == null) return;

            var friendDamages = torpedo.GetFriendDamagesOpen();
            var enemyDamages = torpedo.GetEnemyDamagesOpen();

            this.CalcDamages(friendDamages, true);
            this.CalcDamages(enemyDamages, true);
        }


        private void CalcAirCombatDamages(Api_Kouku airCombat)
        {
            this.CalcDamages(airCombat.GetDamages(FleetType.Friend), true);
            this.CalcDamages(airCombat.GetDamages(FleetType.Enemy), true);
        }

        private void CalcDamages(IEnumerable<Attack> attacks, bool checkDamageControlAfterAllAttacks = false)
        {
            if (attacks == null) return;

            var allTargets = new HashSet<ShipData>();
            foreach (var attack in attacks)
            {
                var source = this.GetShip(attack.Source, attack.isSupport);
                if (source != null)
                {
                    source.AttackDamage += attack.TotalDamage;
                }

                //var targets = new HashSet<ShipData>();
                foreach (var damage in attack.Damages)
                {
                    var target = this.GetShip(damage.Target, attack.isSupport);
                    if (target != null)
                    {
                        //targets.Add(target);
                        allTargets.Add(target);
                        target.ReceiveDamage(damage.Value);
                    }
                }

                if (!checkDamageControlAfterAllAttacks)
                {
                    this.CheckDamageControl(allTargets);
                }
            }

            if (checkDamageControlAfterAllAttacks)
            {
                this.CheckDamageControl(allTargets);
            }
        }

        private ShipData GetShip(int index, bool isSupport = false)
        {
            if (index == 0) return null;

            if (isSupport)
            {
                return index > 0
                ? this.FriendSupportFleet.GetShip(index)
                : this.EnemyFleet.GetShip(-index);
            }
            return index > 0
                ? this.FriendFleet.GetShip(index)
                : this.EnemyFleet.GetShip(-index);
        }

        private void CheckDamageControl(IEnumerable<ShipData> ships)
        {
            if (this.State == BattleState.InSortie)
            {
                foreach (var ship in ships)
                {
                    ship.CheckDamageControl();
                }
            }
        }
    }
}
