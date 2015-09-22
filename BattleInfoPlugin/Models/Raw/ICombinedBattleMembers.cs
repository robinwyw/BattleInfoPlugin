using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Raw
{
    public interface ICombinedBattleMembers : ICommonBattleMembers
    {
        int[] api_nowhps_combined { get; set; }
        int[] api_maxhps_combined { get; set; }
        int[][] api_fParam_combined { get; set; }
    }
}
