using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MetroTrilithon.Serialization;

namespace BattleInfoPlugin.Models.Settings
{
    public static class SettingsHelper
    {
        private static string FilePath { get; } = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "grabacr.net",
            "KanColleViewer",
            "BattleInfoPlugin.xaml");

        public static ISerializationProvider Local { get; } = new FileSettingsProvider(FilePath);

        public static SerializableProperty<T> GenerateProperty<T>(string key, T defaultValue = default(T), bool autoSave = true)
        {
            return new SerializableProperty<T>(key, Local, defaultValue) { AutoSave = autoSave };
        }
    }
}
