using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class PathsSettings
    {
        public string MasterDataFileName { get; } = "BattleInfoPlugin_MasterData.txt";

        public string EnemyDataFileName { get; } = "BattleInfoPlugin_EnemyData.txt";

        public string CacheDirPath { get; } = "BattleInfoPluginData";

        public string ResourceUrlMappingFileName { get; } = "BattleInfoPlugin_ResourceUrlMapping.txt";
    }
}
