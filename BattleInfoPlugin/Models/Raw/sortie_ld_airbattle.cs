using System;

namespace BattleInfoPlugin.Models.Raw
{
    /// <summary>
    /// 長距離空襲戦
    /// </summary>
    public class sortie_ld_airbattle : ICommonFirstBattleMembers, IFleetBattleInfo, IAirStageMembers, IBattleFormationInfo
    {
        private int _apiDeckId;

        public dynamic api_dock_id
        {
            get { return this._apiDeckId; }
            set { this._apiDeckId = value is int ? value : Convert.ToInt32(value); }
        }

        public int api_deck_id
        {
            get { return this._apiDeckId; }
            set { this._apiDeckId = value; }

        }

        public int[] api_ship_ke { get; set; }
        public int[] api_ship_lv { get; set; }
        public int[] api_nowhps { get; set; }
        public int[] api_maxhps { get; set; }
        public int api_midnight_flag { get; set; }
        public int[][] api_eSlot { get; set; }
        public int[][] api_eKyouka { get; set; }
        public int[][] api_fParam { get; set; }
        public int[][] api_eParam { get; set; }
        public int[] api_search { get; set; }
        public int[] api_formation { get; set; }
        public int[] api_stage_flag { get; set; }
        public Api_Kouku api_kouku { get; set; }

        public FleetDamages FirstFleetDamages => this.api_kouku.GetFirstFleetDamages();
        public FleetDamages SecondFleetDamages => null;
        public FleetDamages EnemyDamages => this.api_kouku.GetEnemyDamages();
    }
}
