using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class GetLostItemViewModel : ViewModel
    {
        public bool IsGetItem { get; set; }
        public ItemType Type { get; set; }
        public string ItemName { get; set; }
        public int Count { get; set; }

        public GetLostItemViewModel(GetLostItem item)
        {
            this.IsGetItem = item.Count > 0;
            this.Type = item.Type;
            this.ItemName = item.Type.ToDisplayName();
            this.Count = Math.Abs(item.Count);
        }
    }
}
