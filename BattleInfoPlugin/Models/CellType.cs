using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleInfoPlugin.Models
{
    [Flags]
    public enum CellType
    {
        None = 0,

        開始 = 1 << 0,
        イベント無し = 1 << 1,
        補給 = 1 << 2,
        渦潮 = 1 << 3,
        戦闘 = 1 << 4,
        ボス = 1 << 5,
        気のせい = 1 << 6,
        航空戦 = 1 << 7,
        母港 = 1 << 8,
        揚陸地点 = 1 << 9,
        泊地 = 1 << 10,

        レーダー射撃 = 1 << 27,

        空襲戦 = 1 << 28,
        航空偵察 = 1 << 29,
        能動分岐 = 1 << 30,
        夜戦 = 1 << 31,
    }

    public static class CellTypeExtensions
    {
        public static CellType ToCellType(this int colorNo)
        {
            return (CellType)(1 << colorNo);
        }

        public static CellType ToCellType(this string battleType)
        {
            return battleType.Contains("sp_midnight") ? CellType.夜戦
                : battleType.Contains("ld_airbattle") ? CellType.空襲戦    //ColorNoからも分かるが、航空戦と誤認しないため
                : battleType.Contains("airbattle") ? CellType.航空戦
                : battleType.Contains("ld_shooting") ? CellType.レーダー射撃
                : CellType.None;
        }

        public static CellType GetCellType(this MapCell cell, IReadOnlyDictionary<MapCell, CellType> knownTypes)
        {
            var result = CellType.None;
            if (knownTypes.ContainsKey(cell)) result = result | knownTypes[cell];
            var cellMaster = Repositories.Master.Current.MapCells[cell.Id];
            result = result | cellMaster.ColorNo.ToCellType();
            return result;
        }
    }
}
