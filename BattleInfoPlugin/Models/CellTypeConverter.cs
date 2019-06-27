using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models.Raw;

namespace BattleInfoPlugin.Models
{
    public static class CellTypeConverter
    {
        #region Split CellType

        private static readonly CellType[] SingleTypes
            = ((CellType[])Enum.GetValues(typeof(CellType)))
                .Where(type => type != default(CellType)).ToArray();

        public static IEnumerable<CellType> Split(this CellType type)
        {
            return SingleTypes.Where(single => type.HasFlag(single));
        }

        #endregion

        public static CellType GetCellType(this map_start_next data)
        {
            var type = data.api_event_id.ToCellType();

            if (data.api_event_kind == 2)
            {
                if (data.api_event_id == 6)
                {
                    type = CellType.能動分岐;
                }
                else
                {
                    type |= CellType.夜戦;
                }
            }
            else if (data.api_event_kind == 4)
            {
                type = CellType.航空戦;
            }
            else if (data.api_event_kind == 6 && data.api_event_id != 6)
            {
                type |= CellType.空襲戦;
            }
            else if (data.api_event_kind == 0 && data.api_event_id == 7)
            {
                type = CellType.航空偵察;
            }

            return type;
        }
    }
}
