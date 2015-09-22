using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Raw
{
    public interface IAirStageMembers
    {
        int[] api_stage_flag { get; set; }
        Api_Kouku api_kouku { get; set; }
    }
}
