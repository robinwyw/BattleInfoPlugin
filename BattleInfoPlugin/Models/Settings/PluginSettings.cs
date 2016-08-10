using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Repositories;

namespace BattleInfoPlugin.Models.Settings
{
    public static class PluginSettings
    {
        static PluginSettings()
        {
            var oldPaths = new _PathsSettings();
            var newPaths = new PathsSettings();

            try
            {
                Directory.CreateDirectory(newPaths.BaseDir.ToAbsolutePath());

                TryMoveFile(oldPaths.MasterDataFileName, newPaths.MasterDataFileName);
                TryMoveFile(oldPaths.EnemyDataFileName, newPaths.EnemyDataFileName);
                TryMoveFile(oldPaths.ResourceUrlMappingFileName, newPaths.ResourceUrlMappingFileName);
            }
            catch (UnauthorizedAccessException)
            {
                // cannot access, ignore
            }
        }

        private static void TryMoveFile(string oldPath, string newPath)
        {
            if (File.Exists(oldPath) && !File.Exists(newPath))
                File.Move(oldPath, newPath);
        }

        public static BattleDataSettings BattleData { get; } = new BattleDataSettings();

        public static BattleWindowSettings BattleWindow { get; } = new BattleWindowSettings();

        public static NotifierSettings Notifier { get; } = new NotifierSettings();

        public static PathsSettings Paths { get; } = new PathsSettings();
    }
}
