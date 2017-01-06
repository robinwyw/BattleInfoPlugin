using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin.Models
{
    public class FleetStatus
    {
        public int TotalOriginalHP { get; set; }
        public int TotalNowHP { get; set; }
        public int TotalLostHP { get; set; }
        public int ShipCount { get; set; }
        public int LostCount { get; set; }
        public double LostGauge { get; set; }
    }

    public static class FleetStatusHelper
    {
        public static FleetStatus GetStatus(this BattleFleet fleet)
        {
            var ships = fleet.Fleets
                .SelectMany(f => f.Ships)
                .Where(s => !s.IsInEvacuationOrTow())
                .ToArray();

            var totalOriginalHp = ships.Sum(s => s.OriginalHP);
            var totalNowHp = ships.Sum(s => Math.Max(s.NowHP, 0));
            var totalLostHp = Math.Max(totalOriginalHp - totalNowHp, 0);

            return new FleetStatus
            {
                TotalOriginalHP = totalOriginalHp,
                TotalNowHP = totalNowHp,
                TotalLostHP = totalLostHp,
                ShipCount = ships.Length,
                LostCount = ships.Count(s => s.NowHP <= 0),
                LostGauge = (double)totalLostHp / totalOriginalHp
            };
        }


        public static bool IsInEvacuationOrTow(this ShipData data) =>
            data.Situation.HasFlag(ShipSituation.Evacuation) || data.Situation.HasFlag(ShipSituation.Tow);
    }
}
