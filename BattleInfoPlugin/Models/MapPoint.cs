using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Raw;
using Livet;

namespace BattleInfoPlugin.Models
{
    public class MapPoint : NotificationObject
    {
        public MapId MapId { get; internal set; } 

        public int Id { get; internal set; }

        public CellType Type { get; internal set; }

        internal MapPoint(map_start_next data)
        {
            this.MapId = new MapId(data.api_maparea_id, data.api_mapinfo_no);
            this.Id = data.api_no;
            this.Type = data.ToCellType();
        }
    }

    public struct MapId
    {
        public int AreaId { get; }
        public int InfoIdInArea { get; }

        public MapId(int areaId, int infoIdInArea)
        {
            this.AreaId = areaId;
            this.InfoIdInArea = infoIdInArea;
        }

        public override string ToString()
        {
            return this.AreaId >= 22
                ? "E-" + this.InfoIdInArea
                : this.AreaId + "-" + this.InfoIdInArea;
        }
    }
}
