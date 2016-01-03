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
            this.Type = data.api_event_id.ToCellType();

            if (data.api_event_kind == 2)
            {
                if (data.api_event_id == 6)
                {
                    this.Type = CellType.能動分岐;
                }
                else
                {
                    this.Type |= CellType.夜戦;
                }
            }
            else if (data.api_event_id == 7 && data.api_event_kind == 0)
            {
                this.Type = CellType.航空偵察;
            }
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
