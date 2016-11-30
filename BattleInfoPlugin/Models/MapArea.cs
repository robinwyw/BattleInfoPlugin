using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Repositories;
using Grabacr07.KanColleWrapper.Models.Raw;

namespace BattleInfoPlugin.Models
{
    [DataContract]
    public class MapArea : IEnumerable<MapInfo>
    {
        private static Master Master => Master.Current;

        [DataMember]
        public int Id { get; private set; }

        [DataMember]
        public string Name { get; private set; }

        public MapInfo this[int mapId] => Master.MapInfos.Values
            .FirstOrDefault(info => info.MapAreaId == this.Id &&
                                    info.IdInEachMapArea == mapId);

        public MapArea(kcsapi_mst_maparea maparea)
        {
            this.Id = maparea.api_id;
            this.Name = maparea.api_name;
        }

        #region static members

        public static MapArea Dummy { get; } = new MapArea(new kcsapi_mst_maparea
        {
            api_id = 0,
            api_name = "？？？",
        });

        #endregion

        public IEnumerator<MapInfo> GetEnumerator()
        {
            return Master.MapInfos.Values
                .Where(info => info.MapAreaId == this.Id)
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
