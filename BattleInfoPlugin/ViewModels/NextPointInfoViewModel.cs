using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class NextPointInfoViewModel : ViewModel
    {
        public string MapId { get; set; }

        public int Id { get; set; }

        public CellTypeViewModel CellType { get; set; }

        public bool IsInSortie { get; set; }
    }
}
