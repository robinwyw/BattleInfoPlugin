using System.Runtime.Serialization;
using BattleInfoPlugin.Models.Repositories;

namespace BattleInfoPlugin.Models
{
    [DataContract]
    public class MapCellData
    {
        //セルの色
        [DataMember]
        public int ColorNo { get; set; }
        //吹き出しID
        [DataMember]
        public int CommentKind { get; set; }
        //セルイベントID
        [DataMember]
        public int EventId { get; set; }
        //セルイベント補足
        [DataMember]
        public int EventKind { get; set; }
        //セル番号
        [DataMember]
        public int No { get; set; }
        //エリアID
        [DataMember]
        public int MapAreaId { get; set; }
        //マップNo
        [DataMember]
        public int MapInfoIdInEachMapArea { get; set; }
        //要索敵
        [DataMember]
        public int ProductionKind { get; set; }
        //能動分岐
        [DataMember]
        public int[] SelectCells { get; set; }
        //距離
        [DataMember]
        public int Distance { get; set; }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as MapCellData);
        }

        protected bool Equals(MapCellData other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            return ColorNo == other.ColorNo &&
                CommentKind == other.CommentKind &&
                EventId == other.EventId &&
                EventKind == other.EventKind &&
                No == other.No &&
                MapAreaId == other.MapAreaId &&
                MapInfoIdInEachMapArea == other.MapInfoIdInEachMapArea &&
                ProductionKind == other.ProductionKind &&
                SelectCells.EqualsValue(other.SelectCells) &&
                Distance == other.Distance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ColorNo;
                hashCode = (hashCode * 397) ^ CommentKind;
                hashCode = (hashCode * 397) ^ EventId;
                hashCode = (hashCode * 397) ^ EventKind;
                hashCode = (hashCode * 397) ^ No;
                hashCode = (hashCode * 397) ^ MapAreaId;
                hashCode = (hashCode * 397) ^ MapInfoIdInEachMapArea;
                hashCode = (hashCode * 397) ^ ProductionKind;
                hashCode = (hashCode * 397) ^ SelectCells.GetValuesHashCode();
                hashCode = (hashCode * 397) ^ Distance;
                return hashCode;
            }
        }
    }
}
