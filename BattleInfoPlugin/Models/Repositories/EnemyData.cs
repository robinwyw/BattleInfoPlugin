using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Settings;

namespace BattleInfoPlugin.Models.Repositories
{
    [DataContract]
    internal class EnemyData
    {
        // EnemyId, EnemyMasterIDs
        [DataMember]
        public Dictionary<string, int[]> EnemyDictionary { get; set; } = new Dictionary<string, int[]>();

        // EnemyId, Formation
        [DataMember]
        public Dictionary<string, Formation> EnemyFormation { get; set; } = new Dictionary<string, Formation>();

        // EnemyId, api_eSlot
        [DataMember]
        public Dictionary<string, int[][]> EnemySlotItems { get; set; } = new Dictionary<string, int[][]>();

        // unused in each_battle?
        // EnemyId, api_eKyouka
        [DataMember]
        public Dictionary<string, int[][]> EnemyUpgraded { get; set; } = new Dictionary<string, int[][]>();

        // EnemyId, api_eParam
        [DataMember]
        public Dictionary<string, int[][]> EnemyParams { get; set; } = new Dictionary<string, int[][]>();

        // EnemyId, api_ship_lv
        [DataMember]
        public Dictionary<string, int[]> EnemyLevels { get; set; } = new Dictionary<string, int[]>();

        // EnemyId, MaxHP
        [DataMember]
        public Dictionary<string, int[]> EnemyHPs { get; set; } = new Dictionary<string, int[]>();

        // EnemyId, Name
        [DataMember]
        public Dictionary<string, string> EnemyNames { get; set; } = new Dictionary<string, string>();

        // EnemyId, Rank
        [DataMember(EmitDefaultValue = false)]
        public Dictionary<string, HashSet<int>> EnemyEncounterRank { get; set; }

        [DataMember]
        public Dictionary<string, int> EnemyRank { get; set; } = new Dictionary<string, int>();

        // MapInfoID, CellNo, EnemyId
        [DataMember]
        public Dictionary<int, Dictionary<int, HashSet<string>>> MapEnemyData { get; set; } = new Dictionary<int, Dictionary<int, HashSet<string>>>();

        // MapInfoID, CellNo, BattleApiClassName
        [DataMember]
        public Dictionary<int, Dictionary<int, string>> MapCellBattleTypes { get; set; } = new Dictionary<int, Dictionary<int, string>>();

        // MapInfoID, FromCellNo, ToCellNo
        [DataMember]
        public Dictionary<int, HashSet<KeyValuePair<int, int>>> MapRoute { get; set; } = new Dictionary<int, HashSet<KeyValuePair<int, int>>>();

        // MapInfoID, MapCellData
        [DataMember]
        public Dictionary<int, List<MapCellData>> MapCellDatas { get; set; } = new Dictionary<int, List<MapCellData>>();

    }

    internal class EnemyDataComparer : IEqualityComparer<string>
    {
        private readonly EnemyData enemyData;

        public EnemyDataComparer(EnemyData data)
        {
            this.enemyData = data;
        }

        public bool Equals(string x, string y)
        {
            if (x == y) return true;
            if (x == null || y == null) return false;
            return this.enemyData.EnemyDictionary.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemyDictionary.GetValueOrDefault(y))
                && this.enemyData.EnemyFormation.GetValueOrDefault(x).Equals(this.enemyData.EnemyFormation.GetValueOrDefault(y))
                && this.enemyData.EnemySlotItems.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemySlotItems.GetValueOrDefault(y))
                && this.enemyData.EnemyUpgraded.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemyUpgraded.GetValueOrDefault(y))
                && this.enemyData.EnemyParams.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemyParams.GetValueOrDefault(y))
                && this.enemyData.EnemyLevels.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemyLevels.GetValueOrDefault(y))
                && this.enemyData.EnemyHPs.GetValueOrDefault(x).EqualsValue(this.enemyData.EnemyHPs.GetValueOrDefault(y))
                && this.enemyData.EnemyNames.GetValueOrDefault(x) == this.enemyData.EnemyNames.GetValueOrDefault(y)
                && this.enemyData.EnemyRank.GetValueOrDefault(x) == this.enemyData.EnemyRank.GetValueOrDefault(y);
        }

        public int GetHashCode(string key)
        {
            return this.enemyData.EnemyDictionary.GetValueOrDefault(key).GetValuesHashCode()
                ^ this.enemyData.EnemyFormation.GetValueOrDefault(key).GetHashCode()
                ^ this.enemyData.EnemySlotItems.GetValueOrDefault(key).GetValuesHashCode(x => x.GetValuesHashCode())
                ^ this.enemyData.EnemyUpgraded.GetValueOrDefault(key).GetValuesHashCode(x => x.GetValuesHashCode())
                ^ this.enemyData.EnemyParams.GetValueOrDefault(key).GetValuesHashCode(x => x.GetValuesHashCode())
                ^ this.enemyData.EnemyLevels.GetValueOrDefault(key).GetValuesHashCode()
                ^ this.enemyData.EnemyHPs.GetValueOrDefault(key).GetValuesHashCode()
                ^ (this.enemyData.EnemyNames.GetValueOrDefault(key)?.GetHashCode() ?? 0)
                ^ this.enemyData.EnemyRank.GetValueOrDefault(key).GetHashCode()
                ;
        }
    }
}
