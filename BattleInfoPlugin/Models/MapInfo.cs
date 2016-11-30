using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Grabacr07.KanColleWrapper.Models.Raw;
using BattleInfoPlugin.Models.Repositories;

namespace BattleInfoPlugin.Models
{
    [DataContract]
    public class MapInfo : IEnumerable<MapCell>
    {
        private static Master Master => Master.Current;

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        [DataMember]
        public int MapAreaId { get; private set; }

        public MapArea MapArea => Master.Current.MapAreas[this.MapAreaId] ?? MapArea.Dummy;

        [DataMember]
        public int IdInEachMapArea { get; private set; }

        [DataMember]
        public int Level { get; private set; }

        [DataMember]
        public string OperationName { get; private set; }

        [DataMember]
        public string OperationSummary { get; private set; }

        [DataMember]
        public int RequiredDefeatCount { get; private set; }

        public int Rank { get; internal set; }

        public MapCell this[int cellId] => Master.MapCells.Values
            .FirstOrDefault(c => c.MapAreaId == this.MapAreaId &&
                                 c.MapInfoIdInEachMapArea == this.IdInEachMapArea &&
                                 c.IdInEachMapInfo == cellId);

        public MapInfo(kcsapi_mst_mapinfo mapinfo)
        {
            this.Id = mapinfo.api_id;
            this.Name = mapinfo.api_name;
            this.MapAreaId = mapinfo.api_maparea_id;
            this.IdInEachMapArea = mapinfo.api_no;
            this.Level = mapinfo.api_level;
            this.OperationName = mapinfo.api_opetext;
            this.OperationSummary = mapinfo.api_infotext;
            this.RequiredDefeatCount = mapinfo.api_required_defeat_count ?? 1;
        }

        #region static members

        public static MapInfo Dummy { get; } = new MapInfo(new kcsapi_mst_mapinfo
        {
            api_id = 0,
            api_name = "？？？",
            api_maparea_id = 0,
            api_no = 0,
            api_level = 0,
        });

        #endregion

        public IEnumerator<MapCell> GetEnumerator()
        {
            return Master.MapCells.Values
                .Where(cell => cell.MapAreaId == this.MapAreaId &&
                               cell.MapInfoIdInEachMapArea == this.IdInEachMapArea)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
