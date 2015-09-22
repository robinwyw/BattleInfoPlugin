namespace BattleInfoPlugin.Models.Raw
{
    public interface IFleetBattleInfo
    {
        FleetDamages FirstFleetDamages { get; }
        FleetDamages SecondFleetDamages { get; }
        FleetDamages EnemyDamages { get; }
    }
}
