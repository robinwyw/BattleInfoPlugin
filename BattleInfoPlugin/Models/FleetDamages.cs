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

        public int[] ToArray()
        {
            return this.Ships;
        }

        public FleetDamages(int[] damages = null)
        {
            this.Ships = damages ?? new int[6];
        }

        public static FleetDamages Parse(IEnumerable<int> damages)
        {
            if (damages == null) throw new ArgumentNullException();
            var arr = damages.ToArray();
            if (arr.Length != 6) throw new ArgumentException("艦隊ダメージ配列の長さは6である必要があります。");
            return new FleetDamages(arr);
        }

        public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>) this.Ships).GetEnumerator();

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
            var merged = new FleetDamages();
            for (var i = 0; i < 6; i++)
                merged.Ships[i] = damages.Select(d => d.Ships[i]).Sum();

            return merged;
        }
    }
}
