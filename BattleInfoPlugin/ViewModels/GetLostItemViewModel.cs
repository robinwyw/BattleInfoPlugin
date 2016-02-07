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

        public GetLostItemViewModel(GetLostItem item, bool isGetItem)
        {
            this.IsGetItem = isGetItem;
            this.Type = item.Type;
            this.ItemName = item.Type.ToDisplayName();
            this.Count = item.Count;
        }
    }
}
