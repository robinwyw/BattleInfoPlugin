namespace BattleInfoPlugin.Models.Raw
{
    /// <summary>
    /// 連合艦隊-機動部隊
    /// </summary>
    public class combined_battle_battle : ICombinedBattleMembers, IBattleFormationInfo, IDayBattleMembers, IFleetBattleInfo, ICommonFirstBattleMembers
    {
        public int api_deck_id { get; set; }
        public int[] api_ship_ke { get; set; }
        public int[] api_ship_lv { get; set; }
        public int[] api_nowhps { get; set; }
        public int[] api_maxhps { get; set; }
        public int[] api_nowhps_combined { get; set; }
        public int[] api_maxhps_combined { get; set; }
        public int api_midnight_flag { get; set; }
        public int[][] api_eSlot { get; set; }
        public int[][] api_eKyouka { get; set; }
        public int[][] api_fParam { get; set; }
        public int[][] api_eParam { get; set; }
        public int[][] api_fParam_combined { get; set; }
        public int[] api_search { get; set; }
        public int[] api_formation { get; set; }
        public Api_Air_Base_Attack[] api_air_base_attack { get; set; }
        public int[] api_stage_flag { get; set; }
        public Api_Kouku api_kouku { get; set; }
        public int api_support_flag { get; set; }
        public Api_Support_Info api_support_info { get; set; }
        public int api_opening_flag { get; set; }
        public Raigeki api_opening_atack { get; set; }
        public int[] api_hourai_flag { get; set; }
        public Hougeki api_hougeki1 { get; set; }
        public Raigeki api_raigeki { get; set; }
        public Hougeki api_hougeki2 { get; set; }
        public Hougeki api_hougeki3 { get; set; }

        public FleetDamages FirstFleetDamages => new[]
        {
            this.api_kouku.GetFirstFleetDamages(),
            this.api_hougeki2.GetFriendDamages(),
            this.api_hougeki3.GetFriendDamages()
        }.Merge();

        public FleetDamages SecondFleetDamages => new[]
        {
            this.api_kouku.GetSecondFleetDamages(),
            this.api_opening_atack.GetFriendDamages(),
            this.api_hougeki1.GetFriendDamages(),
            this.api_raigeki.GetFriendDamages()
        }.Merge();

        public FleetDamages EnemyDamages => this.GetEnemyDamages();
    }
}
