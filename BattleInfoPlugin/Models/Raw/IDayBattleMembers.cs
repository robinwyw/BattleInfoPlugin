namespace BattleInfoPlugin.Models.Raw
{
    public interface IDayBattleMembers : IAirStageMembers, IAirBaseAttack
    {
        int api_support_flag { get; set; }
        Api_Support_Info api_support_info { get; set; }
        int api_opening_flag { get; set; }
        Raigeki api_opening_atack { get; set; }
        int[] api_hourai_flag { get; set; }
        Hougeki api_hougeki1 { get; set; }
        Hougeki api_hougeki2 { get; set; }
        Hougeki api_hougeki3 { get; set; }
        Raigeki api_raigeki { get; set; }
    }

    public static class DayBattleMembersExtension
    {
        public static FleetDamages[] GetFriendDamages(this IDayBattleMembers data)
        {
            return new[]
            {
                data.api_kouku.GetFirstFleetDamages(),
                data.api_opening_atack.GetFriendDamages(),
                data.api_hougeki1.GetFriendDamages(),
                data.api_hougeki2.GetFriendDamages(),
                data.api_raigeki.GetFriendDamages()
            };
        }

        public static FleetDamages[] GetEnemyDamages(this IDayBattleMembers data)
        {
            return new[]
            {
                data.api_air_base_attack.GetEnemyDamages(),
                data.api_support_info.GetEnemyDamages(),
                data.api_kouku.GetEnemyDamages(),
                data.api_opening_atack.GetEnemyDamages(),
                data.api_hougeki1.GetEnemyDamages(),
                data.api_hougeki2.GetEnemyDamages(),
                data.api_hougeki3.GetEnemyDamages(),
                data.api_raigeki.GetEnemyDamages()
            };
        }
    }
}
