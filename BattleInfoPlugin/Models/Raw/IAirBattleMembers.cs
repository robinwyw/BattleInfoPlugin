namespace BattleInfoPlugin.Models.Raw
{
    public interface IAirBattleMembers : IAirStageMembers
    {
        int api_support_flag { get; set; }
        Api_Support_Info api_support_info { get; set; }
        int[] api_stage_flag2 { get; set; }
        Api_Kouku api_kouku2 { get; set; }
    }

    public static class AirBattleMembersExtensions
    {
        public static FleetDamages GetFirstFleetDamages(this IAirBattleMembers data)
        {
            return new[]
            {
                data.api_kouku.GetFirstFleetDamages(),
                data.api_kouku2.GetFirstFleetDamages()
            }.Merge();
        }

        public static FleetDamages GetSecondFleetDamages(this IAirBattleMembers data)
        {
            return new[]
            {
                data.api_kouku.GetSecondFleetDamages(),
                data.api_kouku2.GetSecondFleetDamages()
            }.Merge();
        }

        public static FleetDamages GetEnemyDamages(this IAirBattleMembers data)
        {
            return new[]
            {
                data.api_support_info.GetEnemyDamages(), //将来的に増える可能性を想定して追加しておく
                data.api_kouku.GetEnemyDamages(),
                data.api_kouku2.GetEnemyDamages()
            }.Merge();
        }
    }
}
