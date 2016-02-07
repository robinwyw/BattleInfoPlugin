using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleInfoPlugin.Models
{
    public enum ItemType
    {
        Unknown = 0,
        Fuel = 1,
        Ammo = 2,
        Steel = 3,
        Bauxite = 4,
        FlameThrower = 5,
        Bucket = 6,
        BuildKit = 7,
        SBox = 10,
        MBox = 11,
        LBox = 12,
        PresentBox = 60,
    }

    public static class GetItemTypeExtensions
    {
        public static string ToDisplayName(this ItemType type)
        {
            switch (type)
            {
                case ItemType.Fuel:
                    return "燃料";
                case ItemType.Ammo:
                    return "弾薬";
                case ItemType.Steel:
                    return "鋼材";
                case ItemType.Bauxite:
                    return "ボーキ";
                case ItemType.FlameThrower:
                    return "高速建造材";
                case ItemType.Bucket:
                    return "高速修復材";
                case ItemType.BuildKit:
                    return "開発資材";
                case ItemType.SBox:
                    return "家具箱(小)";
                case ItemType.MBox:
                    return "家具箱(中)";
                case ItemType.LBox:
                    return "家具箱(大)";
                case ItemType.PresentBox:
                    return "プレゼント箱";
            }
            return type.ToString();
        }
    }
}
