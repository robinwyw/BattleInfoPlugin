using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grabacr07.KanColleWrapper;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Settings;

namespace BattleInfoPlugin.Models.Repositories
{
    public class EnemyDataProvider
    {
        public static EnemyDataProvider Current { get; } = new EnemyDataProvider(PluginSettings.Paths.EnemyDataFileName);


        internal EnemyData EnemyData { get; private set; }


        private readonly string filePath;

        private readonly object mergeLock = new object();

        public EnemyDataProvider(string filePath)
        {
            this.filePath = filePath;
            this.Reload();
        }

        public void Reload()
        {
            var obj = this.filePath.Deserialize<EnemyData>();
            this.EnemyData = obj.ValueOrNew();

            if (this.RemoveDuplicate())
                this.Save();
        }

        public void Save()
        {
            this.EnemyData.Serialize(this.filePath);
        }

        public bool Merge(string path)
        {
            if (!File.Exists(path)) return false;

            lock (mergeLock)
            {
                var obj = path.Deserialize<EnemyData>();
                if (obj == null) return false;

                this.EnemyData.EnemyDictionary = this.EnemyData.EnemyDictionary.Merge(obj.EnemyDictionary);
                this.EnemyData.EnemyFormation = this.EnemyData.EnemyFormation.Merge(obj.EnemyFormation);
                this.EnemyData.EnemySlotItems = this.EnemyData.EnemySlotItems.Merge(obj.EnemySlotItems);
                this.EnemyData.EnemyUpgraded = this.EnemyData.EnemyUpgraded.Merge(obj.EnemyUpgraded);
                this.EnemyData.EnemyParams = this.EnemyData.EnemyParams.Merge(obj.EnemyParams);
                this.EnemyData.EnemyLevels = this.EnemyData.EnemyLevels.Merge(obj.EnemyLevels);
                this.EnemyData.EnemyHPs = this.EnemyData.EnemyHPs.Merge(obj.EnemyHPs);
                this.EnemyData.EnemyNames = this.EnemyData.EnemyNames.Merge(obj.EnemyNames);
                this.EnemyData.EnemyEncounterRank = this.EnemyData.EnemyEncounterRank.Merge(obj.EnemyEncounterRank);
                this.EnemyData.EnemyRank = this.EnemyData.EnemyRank.Merge(obj.EnemyRank);
                this.EnemyData.MapEnemyData = this.EnemyData.MapEnemyData.Merge(obj.MapEnemyData, (v1, v2) => v1.Merge(v2, (h1, h2) => h1.Merge(h2)));
                this.EnemyData.MapCellBattleTypes = this.EnemyData.MapCellBattleTypes.Merge(obj.MapCellBattleTypes, (v1, v2) => v1.Merge(v2));
                this.EnemyData.MapRoute = this.EnemyData.MapRoute.Merge(obj.MapRoute, (v1, v2) => v1.Merge(v2));
                this.EnemyData.MapCellDatas = this.EnemyData.MapCellDatas.Merge(obj.MapCellDatas, (v1, v2) => v1.Merge(v2, x => x.No));

                this.RemoveDuplicate();
                this.Save();
            }
            return true;
        }

        internal bool RemoveDuplicate()
        {
            var modified = false;
            if (this.EnemyData.EnemyEncounterRank != null)
            {
                this.EnemyData.EnemyRank = this.EnemyData.EnemyRank
                    .Merge(this.EnemyData.EnemyEncounterRank.ToDictionary(x => x.Key, x => x.Value.Max()));

                this.EnemyData.EnemyEncounterRank = null;
                modified = true;
            }

            var keysList = this.EnemyData.MapEnemyData.Values.SelectMany(x => x.Values).ToArray();
            var allKeys = keysList.Merge();

            foreach (var keys in keysList)
            {
                keys.GroupBy(key => key, new EnemyDataComparer(this.EnemyData))
                    .SelectMany(x => x.Skip(1))
                    .ToList()
                    .ForEach(key => keys.Remove(key));
            }

            var restKeys = keysList.Merge();
            allKeys.ExceptWith(restKeys);
            foreach (var key in allKeys)
            {
                this._RemoveEnemy(key);
            }

            return modified || allKeys.Count > 0;
        }

        public void RemoveEnemy(string enemyId)
        {
            this._RemoveEnemy(enemyId);
            this.Save();
        }

        private void _RemoveEnemy(string enemyId)
        {
            this.EnemyData.EnemyDictionary.Remove(enemyId);
            this.EnemyData.EnemyFormation.Remove(enemyId);
            this.EnemyData.EnemySlotItems.Remove(enemyId);
            this.EnemyData.EnemyUpgraded.Remove(enemyId);
            this.EnemyData.EnemyParams.Remove(enemyId);
            this.EnemyData.EnemyLevels.Remove(enemyId);
            this.EnemyData.EnemyHPs.Remove(enemyId);
            this.EnemyData.EnemyNames.Remove(enemyId);
            this.EnemyData.EnemyRank.Remove(enemyId);

            var kvps = this.EnemyData.MapEnemyData
                .SelectMany(x => x.Value);
            foreach (var kvp in kvps)
            {
                kvp.Value.Remove(enemyId);
            }
        }

        public Dictionary<MapInfo, Dictionary<MapCell, Dictionary<string, FleetData>>> GetMapEnemies()
        {
            return this.EnemyData.MapEnemyData
                .Where(x => Master.Current.MapInfos.ContainsKey(x.Key))
                .ToDictionary(
                    info => Master.Current.MapInfos[info.Key],
                    info => info.Value.ToDictionary(
                        cell => Master.Current.MapInfos[info.Key][cell.Key],
                        cell => cell.Value.ToDictionary(
                            enemy => enemy,
                            enemy => (FleetData)this.GetEnemiesFleetsById(enemy)
                        )));
        }

        public Dictionary<MapInfo, Dictionary<MapCell, Dictionary<string, BattleFleet>>> GetMapEnemiesNew()
        {
            return this.EnemyData.MapEnemyData
                .Where(x => Master.Current.MapInfos.ContainsKey(x.Key))
                .ToDictionary(
                    info => Master.Current.MapInfos[info.Key],
                    info => info.Value.ToDictionary(
                        cell => Master.Current.MapInfos[info.Key][cell.Key],
                        cell => cell.Value.ToDictionary(
                            enemy => enemy,
                            this.GetEnemiesFleetsById
                        )));
        }

        public Dictionary<int, Dictionary<int, string>> GetMapCellBattleTypes()
        {
            return this.EnemyData.MapCellBattleTypes;
        }

        public Dictionary<int, List<MapCellData>> GetMapCellDatas()
        {
            return this.EnemyData.MapCellDatas;
        }

        private string GetEnemyNameFromId(string enemyId)
        {
            return this.EnemyData.EnemyNames.ContainsKey(enemyId)
                ? this.EnemyData.EnemyNames[enemyId]
                : "";
        }

        private Formation GetEnemyFormationFromId(string enemyId)
        {
            return this.EnemyData.EnemyFormation.ContainsKey(enemyId)
                ? this.EnemyData.EnemyFormation[enemyId]
                : Formation.不明;
        }

        private int[] GetEnemyEncounterRankFromId(string enemyId)
        {
            return this.EnemyData.EnemyRank.ContainsKey(enemyId)
                ? new[] { this.EnemyData.EnemyRank[enemyId] }
                : new[] { 0 };
        }

        private BattleFleet GetEnemiesFleetsById(string enemy)
        {
            var fleets = new BattleFleet(FleetType.Enemy)
            {
                Name = this.GetEnemyNameFromId(enemy),
                Rank = this.GetEnemyEncounterRankFromId(enemy),
                Formation = this.GetEnemyFormationFromId(enemy)
            };

            fleets.Update(this.GetEnemiesFromId(enemy).Select(f => new FleetData(f)).ToArray());

            return fleets;
        }

        private IEnumerable<IEnumerable<ShipData>> GetEnemiesFromId(string enemyId)
        {
            var shipInfos = KanColleClient.Current.Master.Ships;
            var slotInfos = KanColleClient.Current.Master.SlotItems;
            int[] ids;
            int[][] shipParam;
            int[][] shipSlots;
            int[] lvs;
            int[] hps;
            if (this.EnemyData.EnemyDictionary.TryGetValue(enemyId, out ids) &&
                this.EnemyData.EnemyParams.TryGetValue(enemyId, out shipParam) &&
                this.EnemyData.EnemySlotItems.TryGetValue(enemyId, out shipSlots) &&
                this.EnemyData.EnemyLevels.TryGetValue(enemyId, out lvs) &&
                this.EnemyData.EnemyHPs.TryGetValue(enemyId, out hps))
            {

                var shipIds = SplitData(ids);
                var shipLvs = SplitData(lvs);
                var shipHps = SplitData(hps);

                int[][] upgrades;
                this.EnemyData.EnemyUpgraded.TryGetValue(enemyId, out upgrades);
                if (upgrades != null)
                {
                    for (var i = 0; i < shipParam.Length; i++)
                    {
                        for (var j = 0; j < shipParam[i].Length; j++)
                        {
                            shipParam[i][j] += upgrades[i][j];
                        }
                    }
                }

                var baseIndex = 0;
                return shipIds.Select((x, i) =>
                    {
                        var index = baseIndex;
                        baseIndex += x.Length;
                        return x.Select((id, j) =>
                        {
                            // enemy id now start from 1501
                            id = id > 1500 ? id : id + 1000;
                            var param = shipParam[index + j];
                            return new MastersShipData(shipInfos[id])
                            {
                                Level = shipLvs[i][j],
                                NowHP = shipHps[i][j],
                                MaxHP = shipHps[i][j],
                                Firepower = param[0],
                                Torpedo = param[1],
                                AA = param[2],
                                Armer = param[3],
                                Slots = shipSlots[index + j]
                                    .Where(s => s != -1)
                                    .Select(s => slotInfos[s])
                                    .Select((s, si) => new ShipSlotData(s))
                                    .ToArray()
                            };
                        });
                    })
                    .ToArray();
            }
            else
            {
                return new ShipData[0][];
            }
        }

        private static int[][] SplitData(int[] source)
        {
            Func<int, bool> skipFilter = i => i < 0;
            Func<int, bool> takeFilter = i => i >= 0;

            var result = new List<int[]>();
            var position = 0;

            SkipWhile(source, ref position, skipFilter);

            while (position < source.Length)
            {
                result.Add(TakeWhile(source, ref position, takeFilter));
                SkipWhile(source, ref position, skipFilter);
            }

            return result.ToArray();
        }

        private static void SkipWhile(int[] source, ref int position, Func<int, bool> filter)
        {
            while (position < source.Length && filter(source[position]))
            {
                position++;
            }
        }

        private static int[] TakeWhile(int[] source, ref int position, Func<int, bool> filter)
        {
            var result = new List<int>();
            while (position < source.Length && filter(source[position]))
            {
                result.Add(source[position]);
                position++;
            }

            return result.ToArray();
        }

        internal EnemyDataComparer GetComparer()
        {
            return new EnemyDataComparer(this.EnemyData);
        }
    }
}
