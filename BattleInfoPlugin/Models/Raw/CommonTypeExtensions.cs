using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models.Raw
{
    public static class CommonTypeExtensions
    {
        private static readonly IEnumerable<ShipDamage> EmptyDamages = new ShipDamage[0];

        #region 支援

        public static IEnumerable<ShipDamage> GetDamages(this Api_Support_Info support)
            => support.api_support_airatack?.GetDamages(FleetType.Enemy)
               ?? support.api_support_hourai?.api_damage.GetDamages(FleetType.Enemy)
               ?? EmptyDamages;

        #endregion

        #region 砲撃

        public static IEnumerable<ShipDamage> GetDamages(this Hougeki hougeki, int friendFleetIndex, int enemyFleetIndex)
        {
            return hougeki.api_damage.GetDamages(hougeki.api_at_list, hougeki.api_df_list, friendFleetIndex, enemyFleetIndex);
        }

        public static IEnumerable<ShipDamage> GetDamages(this Midnight_Hougeki hougeki, int friendFleetIndex, int enemyFleetIndex)
        {
            return hougeki.api_damage.GetDamages(hougeki.api_at_list, hougeki.api_df_list, friendFleetIndex, enemyFleetIndex);
        }

        public static IEnumerable<ShipDamage> GetDamages(this Enemy_Combined_Hougeki hougeki)
        {
            var flags = hougeki.api_at_eflag.GetData().ToArray();
            var sources = hougeki.api_at_list.GetData().ToArray();
            var targets = hougeki.api_df_list.GetData().Cast<object[]>().Select(x => Convert.ToInt32(x[0])).ToArray();
            var damages = hougeki.api_damage.GetData().Cast<object[]>().Select(x => x.Sum(Convert.ToInt32)).ToArray();
            for (var i = 0; i < flags.Length; i++)
            {
                int source, target;

                // 0: f->e; 1: e->f
                if (flags[i] == 0)
                {
                    source = ToIndex(sources[i], FleetType.Friend);
                    target = ToIndex(targets[i], FleetType.Enemy);
                }
                else
                {
                    source = ToIndex(sources[i], FleetType.Enemy);
                    target = ToIndex(targets[i], FleetType.Friend);
                }

                yield return new ShipDamage(source, target, damages[i]);
            }
        }

        public static IEnumerable<ShipDamage> GetDamages(
            this object[] apiDamage,
            int[] apiAtList,
            object[] apiDfList,
            int frientFleetIndex,
            int enemyFleetIndex)
        {
            var sources = apiAtList.GetData().ToArray();
            var targets = apiDfList.GetData().Cast<object[]>().Select(x => Convert.ToInt32(x[0])).ToArray();
            var damages = apiDamage.GetData().Cast<object[]>().Select(x => x.Sum(Convert.ToInt32)).ToArray();
            for (var i = 0; i < targets.Length; i++)
            {
                // 1 ~ 12
                var source = sources[i];
                var target = targets[i];

                // 1 ~ 6: e->f; 7 ~ 12: f->e
                var isFriendAttack = source <= 6;

                source = source.To6BasedIndex();
                target = target.To6BasedIndex();
                if (isFriendAttack)
                {
                    source = ToIndex(frientFleetIndex, source, FleetType.Friend);
                    target = ToIndex(enemyFleetIndex, target, FleetType.Enemy);
                    yield return new ShipDamage(source, target, damages[i]);
                }
                else
                {
                    source = ToIndex(enemyFleetIndex, source, FleetType.Enemy);
                    target = ToIndex(frientFleetIndex, target, FleetType.Friend);
                    yield return new ShipDamage(source, target, damages[i]);
                }
            }
        }

        #endregion


        #region 航空戦

        private static readonly Dictionary<FleetType, Func<Api_Stage3, IEnumerable<double>>> Stage3DamageSelector =
           new Dictionary<FleetType, Func<Api_Stage3, IEnumerable<double>>>
           {
               [FleetType.Friend] = stage3 => stage3?.api_fdam.GetData() ?? Enumerable.Empty<double>(),
               [FleetType.Enemy] = stage3 => stage3?.api_edam.GetData() ?? Enumerable.Empty<double>()
           };

        public static IEnumerable<ShipDamage> GetDamages(this Api_Kouku kouku, FleetType type)
        {
            var selector = Stage3DamageSelector[type];
            return selector(kouku.api_stage3)
                .Concat(selector(kouku.api_stage3_combined))
                .GetDamages(type);
        }

        public static bool IsEnabled(this Api_Kouku kouku)
            => kouku?.api_plane_from?
                .Where(arr => arr != null)
                .Any(arr => arr.Any(n => n != -1))
               ?? false;

        public static IEnumerable<ShipDamage> GetDamages(this Api_Air_Base_Attack[] attacks)
        {
            return attacks?.Where(x => x?.api_stage3?.api_edam != null)
                       .SelectMany(kouku => kouku.GetDamages(FleetType.Enemy))
                   ?? EmptyDamages;
        }

        public static AirSupremacy GetAirSupremacy(this Api_Kouku kouku)
            => kouku.IsEnabled()
                ? (AirSupremacy)(kouku.api_stage1?.api_disp_seiku ?? (int)AirSupremacy.航空戦なし)
                : AirSupremacy.航空戦なし;

        public static AirCombatResult[] ToResult(this Api_Kouku kouku, string prefixName = "")
        {
            return kouku.IsEnabled()
                ? new[]
                {
                    kouku.api_stage1.ToResult($"{prefixName}空対空"),
                    kouku.api_stage2.ToResult($"{prefixName}空対艦")
                }
                : new AirCombatResult[0];
        }

        public static LandBaseAirCombatResult[] ToResult(this Api_Air_Base_Attack[] attacks)
        {
            if (attacks == null) return new LandBaseAirCombatResult[0];

            var hashset = new HashSet<int>();
            return attacks
                .Select(attack =>
                {
                    var firstTime = hashset.Add(attack.api_base_id);
                    var index = firstTime ? 1 : 2;

                    return new LandBaseAirCombatResult(
                        $"陸{attack.api_base_id.ToString()}-{index.ToString()}",
                        attack.api_stage1.ToResult("空対空"),
                        attack.api_stage2.ToResult("空対艦"),
                        attack.api_squadron_plane);
                })
                .ToArray();
        }

        public static AirCombatResult ToResult(this Api_Stage_Combat stage, string name)
            => stage == null ? new AirCombatResult(name)
            : new AirCombatResult(name, stage.api_f_count, stage.api_f_lostcount, stage.api_e_count, stage.api_e_lostcount);

        #endregion

        #region 雷撃戦

        public static IEnumerable<ShipDamage> GetFriendDamages(this Raigeki raigeki, int friendFleetIndex, int enemyFleetIndex)
            => raigeki?.api_eydam?
                   .GetDamages(raigeki.api_erai, enemyFleetIndex, friendFleetIndex, FleetType.Enemy, FleetType.Friend)
               ?? EmptyDamages;

        public static IEnumerable<ShipDamage> GetEnemyDamages(this Raigeki raigeki, int friendFleetIndex, int enemyFleetIndex)
            => raigeki?.api_fydam?
                   .GetDamages(raigeki.api_frai, friendFleetIndex, enemyFleetIndex, FleetType.Friend, FleetType.Enemy)
               ?? EmptyDamages;

        #endregion

        #region ダメージ計算

        public static IEnumerable<T> GetData<T>(this IEnumerable<T> source, int origin = 1)
            => source.Skip(origin);

        public static IEnumerable<T> GetSection<T>(this IEnumerable<T> source, int section, int origin = 1)
        {
            return source.GetData(origin).Section(section);
        }

        private static IEnumerable<T> Section<T>(this IEnumerable<T> source, int section)
        {
            return source.Skip(section * 6).Take(6);
        }

        /// <summary>
        /// 12項目中先頭6項目取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="origin">ゴミ-1が付いてる場合1オリジン</param>
        /// <returns></returns>
        public static IEnumerable<T> GetFriendData<T>(this IEnumerable<T> source, int origin = 1)
            => source.GetSection(0, origin);

        /// <summary>
        /// 12項目中末尾6項目取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="origin">ゴミ-1が付いてる場合1オリジン</param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnemyData<T>(this IEnumerable<T> source, int origin = 1)
            => source.GetSection(1, origin);

        /// <summary>
        /// 航空戦ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_fdam/api_edam</param>
        /// <param name="type">friend/enemy</param>
        /// <returns></returns>
        public static IEnumerable<ShipDamage> GetDamages(this IEnumerable<double> damages, FleetType type)
            => damages
                .GetData()
                .Select((x, i) => new ShipDamage(0, ToIndex(i + 1, type), Convert.ToInt32(x)))
                .Where(d => d.Damage > 0);

        /// <summary>
        /// 雷撃戦ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_fydam/api_eydam</param>
        /// <param name="targets">api_frai/api_erai</param>
        /// <param name="sourceFleetIndex">index of source fleet(start from 1)</param>
        /// <param name="targetFleetIndex">index of target fleet(start from 1)</param>
        /// <param name="sourceFleetType">friend/enemy</param>
        /// <param name="targetFleetType">enemy/friend</param>
        /// <returns></returns>
        public static IEnumerable<ShipDamage> GetDamages(
                this double[] damages,
                int[] targets,
                int sourceFleetIndex,
                int targetFleetIndex,
                FleetType sourceFleetType,
                FleetType targetFleetType)
            => damages
                .Zip(targets, (damage, target) => new { target, damage })
                .GetData()
                .Select((x, i) => new ShipDamage(
                    ToIndex(sourceFleetIndex, i + 1, sourceFleetType),
                    ToIndex(targetFleetIndex, x.target, targetFleetType),
                    Convert.ToInt32(x.damage)))
                .Where(d => d.Target != 0);

        #endregion

        private static int To6BasedIndex(this int index)
        {
            return (index - 1) % 6 + 1;
        }

        private static int ToIndex(int fleetIndex, int shipIndex, FleetType type)
        {
            var index = (fleetIndex - 1) * 6 + shipIndex;
            return ToIndex(index, type);
        }

        private static int ToIndex(int index, FleetType type)
        {
            index = Math.Abs(index);
            return type == FleetType.Friend ? index : -index;
        }
    }
}
