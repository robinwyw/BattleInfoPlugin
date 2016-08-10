using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Repositories;

namespace BattleInfoPlugin.Models.Settings
{
    public sealed class PathsSettings
    {
        private static readonly string Dir = "BattleInfoPluginData".ToAbsolutePath();

        public string BaseDir { get; } = Dir;

        public string MasterDataFileName { get; } = $@"{Dir}\MasterData.json";

        public string EnemyDataFileName { get; } = $@"{Dir}\EnemyData.json";

        public string CacheDirPath { get; } = Dir;

        public string ResourceUrlMappingFileName { get; } = $@"{Dir}\ResourceUrlMapping.json";
    }
}
