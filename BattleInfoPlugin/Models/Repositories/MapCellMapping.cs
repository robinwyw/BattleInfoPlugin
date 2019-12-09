using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Repositories
{
    public class MapCellMapping
    {
        public static MapCellMapping Current { get; } = new MapCellMapping();


        #region CellsMapping

        private Dictionary<string, string> _cellsMapping = new Dictionary<string, string>();

        #endregion

        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();


        public string GetCellId(MapId mapId, int cellId)
        {
            return this.GetCellId(mapId.AreaId, mapId.InfoIdInArea, cellId);
        }

        public string GetCellId(int areaNo, int mapId, int cellId)
        {
            string result;

            this._lock.EnterReadLock();

            if (!this._cellsMapping.TryGetValue($"{areaNo}-{mapId}-{cellId}", out result))
            {
                result = cellId.ToString();
            }

            this._lock.ExitReadLock();

            return result;
        }

        public async Task<bool> UpdateData()
        {
            const string url = "https://raw.githubusercontent.com/laserdark/BattleInfoPlugin/master/BattleInfoPlugin/Resources/mapcell.json";

            try
            {
                using (var client = new WebClient())
                using (var jsonDataStream = await client.OpenReadTaskAsync(new Uri(url)))
                {

                    var settings = new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };
                    var deserializer = new DataContractJsonSerializer(typeof(Dictionary<string, Dictionary<string, string>>), settings);

                    var dict = (Dictionary<string, Dictionary<string, string>>)deserializer.ReadObject(jsonDataStream);

                    this._lock.EnterWriteLock();

                    this._cellsMapping = dict
                        .SelectMany(map => map.Value.Select(cell => new { Key = $"{map.Key}-{cell.Key}", cell.Value }))
                        .ToDictionary(p => p.Key, p => p.Value);

                    this._lock.ExitWriteLock();
                }

                return true;
            }
            catch (WebException ex)
            {
                Debug.WriteLine("Fail to update\n" + ex);
                return false;
            }
        }
    }
}
