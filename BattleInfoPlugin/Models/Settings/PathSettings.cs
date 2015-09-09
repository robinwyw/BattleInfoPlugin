using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Settings
{
    public static class PathSettings
    {
        public static string MasterDataFilePath { get; } = "BattleInfoPlugin_MasterData.txt";

        public static string EnemyDataFilePath { get; } = "BattleInfoPlugin_EnemyData.txt";

        public static string CacheDirPath { get; } = "BattleInfoPluginData";
    }
}
