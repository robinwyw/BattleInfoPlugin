namespace BattleInfoPlugin.Models.Raw
{
    public interface IMidnightBattleMembers
    {
        int[] api_touch_plane { get; set; }
        int[] api_flare_pos { get; set; }
        Midnight_Hougeki api_hougeki { get; set; }
    }

    public static class MidnightExtensions
    { 
        public static FleetDamages GetFriendDamages(this IMidnightBattleMembers data)
            => data.api_hougeki.GetFriendDamages();

        public static FleetDamages GetEnemyDamages(this IMidnightBattleMembers data)
            => data.api_hougeki.GetEnemyDamages();
    }
}
