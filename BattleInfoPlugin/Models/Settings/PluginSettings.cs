using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Settings
{
    public static class PluginSettings
    {
        static PluginSettings()
        {
            var oldPaths = new _PathsSettings();
            var newPaths = new PathsSettings();

            Directory.CreateDirectory(newPaths.BaseDir);

            TryMoveFile(oldPaths.MasterDataFileName, newPaths.MasterDataFileName);
            TryMoveFile(oldPaths.EnemyDataFileName, newPaths.EnemyDataFileName);
            TryMoveFile(oldPaths.ResourceUrlMappingFileName, newPaths.ResourceUrlMappingFileName);
        }

        private static void TryMoveFile(string oldPath, string newPath)
        {
            try
            {
                if (File.Exists(oldPath) && !File.Exists(newPath))
                    File.Move(oldPath, newPath);
            }
            catch (UnauthorizedAccessException)
            {
                // cannot access, ignore
            }
        }

        public static BattleDataSettings BattleData { get; } = new BattleDataSettings();

        public static BattleWindowSettings BattleWindow { get; } = new BattleWindowSettings();

        public static NotifierSettings Notifier { get; } = new NotifierSettings();

        public static PathsSettings Paths { get; } = new PathsSettings();
    }
}
