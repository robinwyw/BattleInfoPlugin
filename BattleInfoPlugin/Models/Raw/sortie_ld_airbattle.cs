using System;

namespace BattleInfoPlugin.Models.Raw
{
    /// <summary>
    /// 長距離空襲戦
    /// </summary>
    public class sortie_ld_airbattle : ICommonBattleMembers, IBattleFormationInfo, ICommonFirstBattleMembers
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
        public Api_Kouku api_injection_kouku { get; set; }
        public Api_Air_Base_Attack[] api_air_base_attack { get; set; }
        public int[] api_stage_flag { get; set; }
        public Api_Kouku api_kouku { get; set; }
        public int api_support_flag { get; set; }
        public Api_Support_Info api_support_info { get; set; }

        #region not exists

        int[] ICommonBattleMembers.api_ship_ke_combined { get; set; }
        int[] ICommonBattleMembers.api_ship_lv_combined { get; set; }
        int[] ICommonBattleMembers.api_nowhps_combined { get; set; }
        int[] ICommonBattleMembers.api_maxhps_combined { get; set; }
        int[][] ICommonBattleMembers.api_eSlot_combined { get; set; }
        int[][] ICommonBattleMembers.api_fParam_combined { get; set; }
        int[][] ICommonBattleMembers.api_eParam_combined { get; set; }

        #endregion
    }
}
