using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Localization;
using Livet;

namespace BattleInfoPlugin.ViewModels
{
    public class CellTypeViewModel : ViewModel
    {
        private CellType _CellType;

        private string _CellString;

        public CellTypeViewModel[] SigleCellTypes
        {
            get
            {
                var cellTypes = this._CellType.Split().ToArray();

                if (cellTypes.Length > 1 && cellTypes.Contains(CellType.戦闘))
                {
                    cellTypes = cellTypes.Where(type => type != CellType.戦闘).ToArray();
                }
                return cellTypes.Select(type => new CellTypeViewModel(type)).ToArray();
            }
        }

        public CellType CellType
        {
            get { return this._CellType; }
            set
            {
                if (this._CellType != value)
                {
                    this._CellType = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.SigleCellTypes));
                }
            }
        }

        public string CellString
        {
            get { return this._CellString; }
            set
            {
                if (this._CellString != value)
                {
                    this._CellString = value;
                    this.RaisePropertyChanged();
                    this.RaisePropertyChanged(nameof(this.SigleCellTypes));
                }
            }
        }

        public CellTypeViewModel(CellType type)
        {
            this.CellType = type;
            if(this.CellType != Models.CellType.None)
            {
                CellString = CellTypeLocalization.GetResource(this.CellType);                
            }
            else
            {
                CellString = "None";
            }
                
        }

        public static implicit operator CellType(CellTypeViewModel vm) => vm.CellType;

        public static implicit operator CellTypeViewModel(CellType type) => new CellTypeViewModel(type);
    }
}
