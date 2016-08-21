using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.ViewModels
{
    public interface IAirCombatResultViewModel
    {
        string Name { get; }
        bool IsHappen { get; }
        int Count { get; }
        int LostCount { get; }
        int RemainingCount { get; }
    }
}
