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

        public GetLostItem[] GetItem { get; }

        public GetLostItem LostItem { get; }

        internal MapPoint(map_start_next data)
        {
            this.MapId = new MapId(data.api_maparea_id, data.api_mapinfo_no);
            this.Id = data.api_no;
            this.Type = data.ToCellType();

            var getItems = new List<Api_Itemget>();
            if (data.api_itemget != null)
            {
                getItems.AddRange(data.api_itemget);
            }
            if (data.api_itemget_eo_comment != null)
            {
                getItems.Add(data.api_itemget_eo_comment);
            }
            if (data.api_itemget_eo_result != null)
            {
                getItems.Add(data.api_itemget_eo_result);
            }

            this.GetItem = getItems.Select(item => new GetLostItem(item)).ToArray();
            this.LostItem = data.api_happening;
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

    public class GetLostItem
    {
        public ItemType Type { get; }
        public int Count { get; }

        public GetLostItem(Api_Itemget itemget)
        {
            this.Type = (ItemType)itemget.api_id;
            this.Count = itemget.api_getcount;
        }

        public GetLostItem(Api_Happening happening)
        {
            this.Type = (ItemType)happening.api_mst_id;
            this.Count = happening.api_count;
        }

        public static implicit operator GetLostItem(Api_Itemget itemget)
        {
            if (itemget == null) return null;
            return new GetLostItem(itemget);
        }

        public static implicit operator GetLostItem(Api_Happening happening)
        {
            if (happening == null) return null;
            return new GetLostItem(happening);
        }
    }
}
