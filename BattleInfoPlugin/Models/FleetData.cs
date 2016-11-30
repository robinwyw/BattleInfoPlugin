using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Livet;
using Grabacr07.KanColleWrapper.Models;

namespace BattleInfoPlugin.Models
{
    public class FleetData : NotificationObject
    {
        public int Index { get; set; }

        #region FleetType変更通知プロパティ
        private FleetType _FleetType;

        public FleetType FleetType
        {
            get
            { return this._FleetType; }
            set
            {
                if (this._FleetType == value)
                    return;
                this._FleetType = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region Name変更通知プロパティ
        private string _Name;

        public string Name
        {
            get
            { return this._Name; }
            set
            {
                if (this._Name == value)
                    return;
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        public int ShipCount => this.Ships.Count;

        public IReadOnlyList<ShipData> Ships { get; }

        #region Formation変更通知プロパティ
        private Formation _Formation;

        public Formation Formation
        {
            get
            { return this._Formation; }
            set
            {
                if (this._Formation == value)
                    return;
                this._Formation = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        #region Rank変更通知プロパティ
        private int[] _Rank;

        public int[] Rank
        {
            get
            { return this._Rank; }
            set
            {
                if (this._Rank == value)
                    return;
                this._Rank = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion

        public FleetData(IEnumerable<ShipData> ships)
        {
            this.Ships = new ItemsCollection<ShipData>(ships);
        }

        public FleetData(IEnumerable<ShipData> ships, string name, FleetType type, int[] rank = null)
        {
            this.Ships = new ItemsCollection<ShipData>(ships);
            this._Name = name;
            this._FleetType = type;
            this._Rank = rank ?? new[] { 0 };
        }

        public static FleetData EmptyFriendFleet { get; } = new FleetData(new ShipData[0], "", FleetType.Friend);

        public static FleetData EmptyEnemyFleet { get; } = new FleetData(new ShipData[0], "", FleetType.Enemy);
    }

    public static class FleetDataExtensions
    {
        /// <summary>
        /// Actionを使用して値を設定
        /// Zipするので要素数が少ない方に合わせられる
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <param name="setter"></param>
        public static void SetValues<TSource, TValue>(
            this IEnumerable<TSource> source,
            IEnumerable<TValue> values,
            Action<TSource, TValue> setter)
        {
            source.Zip(values, (s, v) => new { s, v })
                .ToList()
                .ForEach(x => setter(x.s, x.v));
        }

        internal static void UpdateHPs(this FleetData fleet, IEnumerable<int> maxhps, IEnumerable<int> nowhps)
        {
            fleet.UpdateMaxHPs(maxhps);
            fleet.UpdateNowHPs(nowhps);
        }

        internal static void UpdateMaxHPs(this FleetData fleet, IEnumerable<int> maxhps)
        {
            fleet.Ships.SetValues(maxhps, (s, v) => s.MaxHP = v);
        }

        internal static void UpdateNowHPs(this FleetData fleet, IEnumerable<int> nowhps)
        {
            fleet.Ships.SetValues(nowhps, (s, v) => s.NowHP = v);
        }

        /// <summary>
        /// ダメージ適用
        /// </summary>
        /// <param name="fleet">艦隊</param>
        /// <param name="damages">適用ダメージリスト</param>
        public static void CalcDamages(this FleetData fleet, params FleetDamages[] damages)
        {
            if (damages == null) return;
            if (fleet == null) return;

            foreach (var damage in damages.Where(d => d != null))
            {
                for (var i = 0; i < fleet.ShipCount; i++)
                {
                    fleet.Ships[i].ReceiveDamage(damage.Ships[i]);
                }
                // fleet.SetValues(damage, (s, d) => s.ReceiveDamage(d));

                if (fleet.FleetType == FleetType.Enemy) continue;

                // ダメコンによる回復処理。同一戦闘で2度目が発生する事はないという前提……
                // ダメコン優先度: 拡張スロット＞インデックス順
                var dameconState = fleet.Ships.Select(x => new { HasDamecon = x.HasDamecon(), HasMegami = x.HasMegami() });
                fleet.Ships.SetValues(dameconState, (s, d) =>
                {
                    if (0 < s.NowHP) return;
                    s.IsUsedDamecon = d.HasDamecon || d.HasMegami;
                    if (d.HasDamecon)
                        s.NowHP = (int)Math.Floor(s.MaxHP * 0.2);
                    else if (d.HasMegami)
                        s.NowHP = s.MaxHP;
                });
            }
        }

        /// <summary>
        /// 演習ダメージ適用
        /// </summary>
        /// <param name="fleet">艦隊</param>
        /// <param name="damages">適用ダメージリスト</param>
        public static void CalcPracticeDamages(this FleetData fleet, params FleetDamages[] damages)
        {
            if (damages == null) return;

            foreach (var damage in damages)
            {
                fleet.Ships.SetValues(damage, (s, d) => s.ReceiveDamage(d));
            }
        }

        private static bool HasDamecon(this ShipData ship)
        {
            return ship?.ExSlot?.Source?.Id == 42
                || ship?.FirstDameconOrNull()?.Source?.Id == 42;
        }

        private static bool HasMegami(this ShipData ship)
        {
            return ship?.ExSlot?.Source?.Id == 43
                || ship?.FirstDameconOrNull()?.Source?.Id == 43;
        }

        private static ShipSlotData FirstDameconOrNull(this ShipData ship)
        {
            return ship?.Slots?.FirstOrDefault(x => x?.Source?.Id == 42 || x?.Source?.Id == 43);
        }
    }
}
