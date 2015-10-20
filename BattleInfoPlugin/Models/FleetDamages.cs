using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models
{
    /// <summary>
    /// 1艦隊分のダメージ一覧
    /// </summary>
    public class FleetDamages : IEnumerable<int>
    {
        public int[] Ships { get; }

        public FleetDamages(IEnumerable<int> damages = null)
        {
            this.Ships = damages?.ToArray() ?? new int[6];
            if (this.Ships.Length != 6) throw new ArgumentException("艦隊ダメージ配列の長さは6である必要があります。");
        }

        public static FleetDamages Parse(IEnumerable<int> damages)
        {
            if (damages == null) throw new ArgumentNullException();
            return new FleetDamages(damages);
        }

        public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)this.Ships).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    public static class FleetDamagesExtensions
    {
        public static FleetDamages ToFleetDamages(this IEnumerable<int> damages)
        {
            return FleetDamages.Parse(damages);
        }

        public static FleetDamages Merge(this FleetDamages[] damages)
        {
            var damage = new int[6];
            var damagesWithoutNull = damages.Where(d => d != null).ToArray();
            for (var i = 0; i < 6; i++)
                damage[i] = damagesWithoutNull.Select(d => d.Ships[i]).Sum();

            return new FleetDamages(damage);
        }
    }
}
