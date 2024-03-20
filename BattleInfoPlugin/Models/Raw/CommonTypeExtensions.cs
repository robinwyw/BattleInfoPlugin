﻿using System;
using System.Collections.Generic;
using System.Linq;
using BattleInfoPlugin.Properties;

namespace BattleInfoPlugin.Models.Raw
{
    public static class CommonTypeExtensions
    {
        private static readonly IEnumerable<Attack> EmptyDamages = new Attack[0];

        #region 支援

        public static IEnumerable<Attack> GetDamages(this Api_Support_Info support, int supportType)
            => support.api_support_airatack?.GetDamages(FleetType.Enemy)
               ?? support.api_support_hourai?.GetDamages(supportType)
               ?? EmptyDamages;

        public static IEnumerable<Attack> GetDamages(this Api_Support_Hourai support, int supportType)
        {
            HashSet<Attack> output = new HashSet<Attack>();
            for (int index = 0; index < support.api_damage.Length; index++)
            {
                int target = ToIndex(index, FleetType.Enemy) - 1;
                int damage = Convert.ToInt32(support.api_damage[index]);
                bool isCritical = support.api_cl_list[index] > 0;
                output.Add(new Attack(0, target, damage, isCritical));
            }
            return output.AsEnumerable<Attack>();
        }
        //{
        //    // supportType = 2 -> isCritical = 1
        //    // supportType = 3 -> isCritical = 2
        //    var criticalFlag = 2;
        //    return support.api_damage
        //        .Select((damage, index) => new
        //        {
        //            target = ToIndex(index, FleetType.Enemy) -1,
        //            damage = Convert.ToInt32(damage),
        //            critical = support.api_cl_list[index]
        //        })
        //        .GetData()
        //        .Select((x, i) => new Attack(0, x.target, x.damage, x.critical == criticalFlag));
        //}

        #endregion

        #region 砲撃

        public static IEnumerable<Attack> GetDamages(this Hougeki hougeki)
        {
            return hougeki.api_damage.GetDamages(hougeki.api_at_eflag, hougeki.api_at_list, hougeki.api_df_list, hougeki.api_cl_list);
        }

        public static IEnumerable<Attack> GetDamages(this Midnight_Hougeki hougeki, bool friendlySupport)
        {
            return hougeki.api_damage.GetDamages(hougeki.api_at_eflag, hougeki.api_at_list, hougeki.api_df_list, hougeki.api_cl_list, friendlySupport);
        }

        public static IEnumerable<Attack> GetDamages(this Enemy_Combined_Hougeki hougeki)
        {
            return hougeki.api_damage.GetDamages(hougeki.api_at_eflag, hougeki.api_at_list, hougeki.api_df_list, hougeki.api_cl_list);
        }

        public static IEnumerable<Attack> GetDamages(
            this object[] apiDamage,
            int[] apiAtFlags,
            int[] apiAtList,
            object[] apiDfList,
            object[] apiClList,
            bool friendlySupport = false)
        {
            var flags = apiAtFlags.GetData().ToArray();
            var sources = apiAtList.GetData().ToArray();
            var targets = apiDfList.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();
            var damages = apiDamage.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();
            var criticals = apiClList.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();

            for (var i = 0; i < flags.Length; i++)
            {
                FleetType sourceType, targetType;

                // 0: f->e; 1: e->f
                if (flags[i] == 0)
                {
                    sourceType = FleetType.Friend;
                    targetType = FleetType.Enemy;
                }
                else
                {
                    sourceType = FleetType.Enemy;
                    targetType = FleetType.Friend;
                }

                var source = sourceType == FleetType.Enemy ? -sources[i] - 1 : sources[i] + 1;
                //ToIndex(sources[i], sourceType);
                var attackDamages = damages[i].Select((damage, index) =>
                            new Damage(targetType == FleetType.Enemy ? -targets[i][index] - 1 : targets[i][index] + 1, damage, criticals[i][index] == 2))
                    .ToArray();

                yield return new Attack(source, attackDamages, friendlySupport);
            }

            //var sources = apiAtList.GetData().ToArray();
            //var targets = apiDfList.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();
            //var damages = apiDamage.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();
            //var criticals = apiClList.GetData().Cast<object[]>().Select(x => x.ToInt32Array()).ToArray();
            //for (var i = 0; i < targets.Length; i++)
            //{
            //    // 1 ~ 12
            //    var rawSource = sources[i];
            //    var attackTargets = targets[i];
            //    var critical = criticals[i];

            //    // 1 ~ 6: f->e; 7 ~ 12: e->f
            //    var isFriendAttack = rawSource <= 6;

            //    int sourceFleetIndex, targetFleetIndex;
            //    if (isFriendAttack)
            //    {
            //        sourceFleetIndex = friendFleetIndex;
            //        targetFleetIndex = enemyFleetIndex;
            //    }
            //    else
            //    {
            //        sourceFleetIndex = enemyFleetIndex;
            //        targetFleetIndex = friendFleetIndex;
            //    }

            //    var source = ToHougekiIndex(sourceFleetIndex, rawSource);
            //    var attackDamages = damages[i].Select((damage, index) =>
            //    {
            //        var target = ToHougekiIndex(targetFleetIndex, attackTargets[index]);
            //        return new Damage(target, damage, critical[index] == 2);
            //    });

            //    yield return new Attack(source, attackDamages);
            //}
        }

        #endregion


        #region 航空戦

        private static readonly Dictionary<FleetType, Func<Api_Stage3, IEnumerable<Tuple<int, int, int, int>>>> Stage3DamageSelector =
           new Dictionary<FleetType, Func<Api_Stage3, IEnumerable<Tuple<int, int, int, int>>>>
           {
               [FleetType.Friend] = stage3 => stage3?.api_fdam?
                                                 .Select((x, i) => new Tuple<int, int, int, int>(Convert.ToInt32(stage3.api_frai_flag[i]), Convert.ToInt32(stage3.api_fbak_flag[i]), Convert.ToInt32(x), Convert.ToInt32(stage3.api_fcl_flag[i])))
                                                 .GetData()
                                             ?? Enumerable.Empty<Tuple<int, int, int, int>>(),
               [FleetType.Enemy] = stage3 => stage3?.api_edam?
                                                 .Select((x, i) => new Tuple<int, int, int, int>(Convert.ToInt32(stage3.api_erai_flag[i]), Convert.ToInt32(stage3.api_ebak_flag[i]), Convert.ToInt32(x), Convert.ToInt32(stage3.api_ecl_flag[i])))
                                                 .GetData()
                                             ?? Enumerable.Empty<Tuple<int, int, int, int>>()
           };

        public static IEnumerable<Attack> GetDamages(this Api_Kouku kouku, FleetType type)
        {
            var selector = Stage3DamageSelector[type];
            return selector(kouku.api_stage3)
                .Concat(selector(kouku.api_stage3_combined))
                .Select((x, i) => new
                {
                    target = type == FleetType.Enemy ? ToKoukuIndex(i, type) -1  : ToKoukuIndex(i , type) + 1,
                    happened = x.Item1 == 1 || x.Item2 == 1,
                    damage = x.Item3,
                    isCriticalHit = x.Item4 == 1
                })
                .Where(x => x.happened)
                .Select(x => new Attack(0, x.target, x.damage, x.isCriticalHit));
        }

        public static bool IsEnabled(this Api_Kouku kouku)
            => kouku?.api_plane_from?
                .Where(arr => arr != null)
                .Any(arr => arr.Any(n => n != -1))
               ?? false;

        public static IEnumerable<Attack> GetDamages(this Api_Air_Base_Attack[] attacks)
        {
            return attacks?.SelectMany(kouku => kouku.GetDamages(FleetType.Enemy))
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
                    kouku.api_stage1.ToResult($"{prefixName}" + Resources.Air_To_Air),
                    kouku.api_stage2.ToResult($"{prefixName}" + Resources.Air_To_Ship)
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
                        attack.api_stage1.ToResult(Resources.Air_To_Air),
                        attack.api_stage2.ToResult(Resources.Air_To_Ship),
                        attack.api_squadron_plane);
                })
                .ToArray();
        }

        public static AirCombatResult ToResult(this Api_Stage_Combat stage, string name)
            => stage == null ? new AirCombatResult(name)
            : new AirCombatResult(name, stage.api_f_count, stage.api_f_lostcount, stage.api_e_count, stage.api_e_lostcount);

        #endregion

        #region 雷撃戦

        public static IEnumerable<Attack> GetFriendDamages(this Raigeki raigeki)
            => raigeki?.api_eydam?
                   .GetDamages(raigeki.api_erai, raigeki.api_ecl, FleetType.Enemy, FleetType.Friend)
               ?? EmptyDamages;

        public static IEnumerable<Attack> GetEnemyDamages(this Raigeki raigeki)
            => raigeki?.api_fydam?
                   .GetDamages(raigeki.api_frai, raigeki.api_fcl, FleetType.Friend, FleetType.Enemy)
               ?? EmptyDamages;


        public static IEnumerable<Attack> GetFriendDamagesOpen(this Raigeki raigeki)
    => raigeki?.api_eydam_list_items?
           .GetDamagesOpen(raigeki.api_erai_list_items, raigeki.api_ecl_list_items, FleetType.Enemy, FleetType.Friend)
       ?? EmptyDamages;

        public static IEnumerable<Attack> GetEnemyDamagesOpen(this Raigeki raigeki)
            => raigeki?.api_fydam_list_items?
                   .GetDamagesOpen(raigeki.api_frai_list_items, raigeki.api_fcl_list_items, FleetType.Friend, FleetType.Enemy)
               ?? EmptyDamages;

        /// <summary>
        /// 雷撃戦ダメージリスト算出
        /// </summary>
        /// <param name="damages">api_fydam/api_eydam</param>
        /// <param name="targets">api_frai/api_erai</param>
        /// <param name="criticals">api_fcl/api_ecl</param>
        /// <param name="sourceFleetIndex">index of source fleet(start from 1)</param>
        /// <param name="targetFleetIndex">index of target fleet(start from 1)</param>
        /// <param name="sourceFleetType">friend/enemy</param>
        /// <param name="targetFleetType">enemy/friend</param>
        /// <returns></returns>
        public static IEnumerable<Attack> GetDamages(
        this double[] damages,
        int[] targets,
        int[] criticals,
        FleetType sourceFleetType,
        FleetType targetFleetType)
        {
            HashSet<Attack> output = new HashSet<Attack>();
            for (int index = 0; index < damages.Length; index++)
            {
                    if (targets[index] != -1)
                    {
                        int source = sourceFleetType == FleetType.Friend ? index : -index;
                        int target = sourceFleetType == FleetType.Enemy ? targets[index] + 1 : -targets[index] - 1;
                        int damage = Convert.ToInt32(damages[index]);
                        bool isCritical = criticals[index] > 0;
                        output.Add(new Attack(source, target, damage, isCritical));
                    }
            }
            return output.AsEnumerable<Attack>();
        }

        public static IEnumerable<Attack> GetDamagesOpen(this double[][] damages,
            int[][] targets,
            int[][] criticals,
            FleetType sourceFleetType,
            FleetType targetFleetType)
        {
            HashSet<Attack> output = new HashSet<Attack>();
            for (int i = 0; i < damages.Length; i++)
            {
                for (int j = 0; j < damages.Length; ++j)
                {
                    if (targets[i][j] != -1)
                    {
                        int source = sourceFleetType == FleetType.Friend ? i : -i;
                        int target = sourceFleetType == FleetType.Enemy ? targets[i][j] + 1 : -targets[i][j] - 1;
                        int damage = Convert.ToInt32(damages[i][j]);
                        bool isCritical = criticals[i][j] > 0;
                        output.Add(new Attack(source, target, damage, isCritical));
                    }
                }
            }
            return output.AsEnumerable<Attack>();
        }
        //=> targets
        //    .Select((target, index) => new
        //    {
        //        source = ToIndex(sourceFleetIndex, index, sourceFleetType),
        //        target = ToIndex(targetFleetIndex, target, targetFleetType),
        //        damage = Convert.ToInt32(damages[index]),
        //        isCritical = criticals[index] > 0
        //    })
        //    .GetData()
        //    .Where(x => x.target != -1)
        //    .Select(x => new Attack(x.source, x.target, x.damage, x.isCritical));

        #endregion

        #region ダメージ計算

        public static IEnumerable<T> GetData<T>(this IEnumerable<T> source, int origin = 0)
            => source.Skip(origin);

        public static IEnumerable<T> GetSection<T>(this IEnumerable<T> source, int section, int origin = 0)
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
        public static IEnumerable<T> GetFriendData<T>(this IEnumerable<T> source, int origin = 0)
            => source.GetSection(0, origin);

        /// <summary>
        /// 12項目中末尾6項目取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="origin">ゴミ-1が付いてる場合1オリジン</param>
        /// <returns></returns>
        public static IEnumerable<T> GetEnemyData<T>(this IEnumerable<T> source, int origin = 0)
            => source.GetSection(0, origin);

        #endregion

        private static int FleetOffset(int index, FleetType type)
        {
            return type == FleetType.Friend ? (index + 1 > BattleData.Current.FriendFleet.Fleets[1].ShipCount ? To6BasedIndex(index, type) : index)
                                            : (index + 1 > BattleData.Current.EnemyFleet.Fleets[1].ShipCount ? -To6BasedIndex(index, type) : -index);
            //((fleetIndex - 1) * BattleData.Current.FriendFleet.Fleets[0].Ships.Count) + index : ((fleetIndex - 1) * BattleData.Current.FriendFleet.Fleets[0].Ships.Count) + index;
        }

        private static int To6BasedIndex(int index, FleetType type)
        {
            return type == FleetType.Friend ? index - BattleData.Current.FriendFleet.Fleets[1].ShipCount + 6
                                            : index - BattleData.Current.EnemyFleet.Fleets[1].ShipCount + 6;
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

        //private static int ToHougekiIndex(int fleetIndex, int shipIndex)
        //{
        //    // 1 ~ 6: friend; 6 ~ 12 : enemy
        //    var fleetType = shipIndex <= 6 ? FleetType.Friend : FleetType.Enemy;
        //    return ToIndex(fleetIndex, shipIndex.To6BasedIndex(), fleetType);
        //}

        private static int ToKoukuIndex(int index, FleetType type)
        {
            index = Math.Abs(index);
            return FleetOffset(index, type);
        }

        private static int[] ToInt32Array(this object objArr)
        {
            return ((object[])objArr).Select(Convert.ToInt32).ToArray();
        }
    }
}
