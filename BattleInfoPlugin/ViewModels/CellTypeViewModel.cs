using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class CellTypeViewModel : ViewModel
    {
        private CellType _CellType;

        public CellType CellType
        {
            get { return this._CellType; }
            set
            {
                if (this._CellType != value)
                {
                    this._CellType = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public CellTypeViewModel(CellType type)
        {
            this.CellType = type;
        }

        public static implicit operator CellType(CellTypeViewModel vm) => vm.CellType;

        public static implicit operator CellTypeViewModel(CellType type) => new CellTypeViewModel(type);
    }
}
