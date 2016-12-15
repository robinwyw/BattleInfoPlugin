using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Raw
{

    public class member_practice_enemyinfo
    {
        public int api_member_id { get; set; }
        public string api_nickname { get; set; }
        public string api_nickname_id { get; set; }
        public string api_cmt { get; set; }
        public string api_cmt_id { get; set; }
        public int api_level { get; set; }
        public int api_rank { get; set; }
        public int[] api_experience { get; set; }
        public int api_friend { get; set; }
        public int[] api_ship { get; set; }
        public int[] api_slotitem { get; set; }
        public int api_furniture { get; set; }
        public string api_deckname { get; set; }
        public string api_deckname_id { get; set; }
        public api_deck api_deck { get; set; }
    }

    public class api_deck
    {
        public api_ship[] api_ships { get; set; }
    }

    public class api_ship
    {
        public int api_id { get; set; }
        public int api_ship_id { get; set; }
        public int api_level { get; set; }
        public int api_star { get; set; }
    }
}
