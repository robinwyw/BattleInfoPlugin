using Grabacr07.KanColleWrapper.Models.Raw;

namespace BattleInfoPlugin.Models.Raw
{
    public class map_anchorage_repair
    {
        public int[] api_used_ship { get; set; }
        public int[] api_repair_ships { get; set; }
        public kcsapi_ship2[] api_ship_data { get; set; }
    }
}
