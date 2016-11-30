using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Raw;
using BattleInfoPlugin.Models.Repositories;
using Grabacr07.KanColleWrapper;
using Livet;
using Master = BattleInfoPlugin.Models.Repositories.Master;

namespace BattleInfoPlugin.Models
{
    public class MapCellInfo : NotificationObject
    {
        public MapId MapId { get; internal set; }

        public int Id { get; internal set; }

        public CellType Type { get; internal set; }

        public IEnumerable<GetLostItem> GetLostItems { get; }

        public FleetData[] KnownEnemies { get; }

        internal MapCellInfo(map_start_next data)
        {
            this.MapId = new MapId(data.api_maparea_id, data.api_mapinfo_no);
            this.Id = data.api_no;
            this.Type = data.GetCellType();

            var getLostItems = new List<GetLostItem>();
            if (data.api_itemget != null)
            {
                getLostItems.AddRange(data.api_itemget.Select(item => new GetLostItem(item)));
            }
            if (data.api_itemget_eo_comment != null)
            {
                getLostItems.Add(data.api_itemget_eo_comment);
            }
            if (data.api_itemget_eo_result != null)
            {
                getLostItems.Add(data.api_itemget_eo_result);
            }
            if (data.api_happening != null)
            {
                getLostItems.Add(data.api_happening);
            }

            this.GetLostItems = getLostItems;

            var info = Master.Current.MapAreas[data.api_maparea_id][data.api_mapinfo_no];
            var cell = info[data.api_no];

            if (cell != null)
            {
                var rank = info.Rank;
                this.KnownEnemies = EnemyDataProvider.Current
                    .GetMapEnemies()
                    .GetOrAddNew(info)
                    .GetOrAddNew(cell).Values
                    .Where(f => f.Rank?.Contains(rank) ?? false)
                    .ToArray();
            }
            else
            {
                this.KnownEnemies = new FleetData[0];
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
            if (KanColleClient.Current.Master.MapAreas[this.AreaId].RawData.api_type == 0)
            {
                return $"{this.AreaId}-{this.InfoIdInArea}";
            }
            else
            {
                return $"E-{this.InfoIdInArea}";
            }
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
            this.Count = -happening.api_count;
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
