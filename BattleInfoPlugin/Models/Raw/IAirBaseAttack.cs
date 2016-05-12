using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models.Raw
{
    public interface IAirBaseAttack
    {
        Api_Air_Base_Attack[] api_air_base_attack { get; set; }
    }
}
