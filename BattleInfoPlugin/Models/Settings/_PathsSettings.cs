using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Repositories;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class _PathsSettings
    {
        public string MasterDataFileName { get; } = "BattleInfoPlugin_MasterData.txt".ToAbsolutePath();

        public string EnemyDataFileName { get; } = "BattleInfoPlugin_EnemyData.txt".ToAbsolutePath();

        public string CacheDirPath { get; } = "BattleInfoPluginData".ToAbsolutePath();

        public string ResourceUrlMappingFileName { get; } = "BattleInfoPlugin_ResourceUrlMapping.txt".ToAbsolutePath();
    }
}
