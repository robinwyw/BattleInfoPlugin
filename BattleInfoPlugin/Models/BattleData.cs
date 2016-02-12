using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using BattleInfoPlugin.Models.Raw;
using BattleInfoPlugin.Models.Repositories;
using Grabacr07.KanColleWrapper;
using Livet;

namespace BattleInfoPlugin.Models
{
    public class BattleData : NotificationObject
    {
        public static BattleData Current { get; } = new BattleData();

        private BattleState _State;

        public BattleState State
        {
            get { return this._State; }
            set
            {
                if (this._State != value)
                {
                    this._State = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private bool _IsInBattle;

        public bool IsInBattle
        {
            get { return this._IsInBattle; }
            internal set
            {
                if (this._IsInBattle != value)
                {
                    this._IsInBattle = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private BattleResult _BattleResult;

        public BattleResult BattleResult
        {
            get { return this._BattleResult; }
            internal set
            {
                if (this._BattleResult != value)
                {
                    this._BattleResult = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private MapPoint _NextCell;

        public MapPoint NextCell
        {
            get { return this._NextCell; }
            set
            {
                if (this._NextCell != value)
                {
                    this._NextCell = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        //FIXME 敵の開幕雷撃&連合艦隊がまだ不明(とりあえず第二艦隊が受けるようにしてる)

        #region Name変更通知プロパティ
        private string _Name;

        public string Name
        {
            get
            { return this._Name; }
            set
            {
                if (this._Name == value)
                    return;
                this._Name = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region UpdatedTime変更通知プロパティ
        private DateTimeOffset _UpdatedTime;

        public DateTimeOffset UpdatedTime
        {
            get
            { return this._UpdatedTime; }
            set
            {
                if (this._UpdatedTime == value)
                    return;
                this._UpdatedTime = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region BattleSituation変更通知プロパティ

        private BattleSituation _BattleSituation;

        public BattleSituation BattleSituation
        {
            get
            { return this._BattleSituation; }
            set
            {
                if (this._BattleSituation == value)
                    return;
                this._BattleSituation = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region FirstFleet変更通知プロパティ
        private FleetData _FirstFleet;

        public FleetData FirstFleet
        {
            get
            { return this._FirstFleet; }
            set
            {
                if (this._FirstFleet == value)
                    return;
                this._FirstFleet = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region SecondFleet変更通知プロパティ
        private FleetData _SecondFleet;

        public FleetData SecondFleet
        {
            get
            { return this._SecondFleet; }
            set
            {
                if (this._SecondFleet == value)
                    return;
                this._SecondFleet = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region Enemies変更通知プロパティ
        private FleetData _Enemies;

        public FleetData Enemies
        {
            get
            { return this._Enemies; }
            set
            {
                if (this._Enemies == value)
                    return;
                this._Enemies = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region FriendAirSupremacy変更通知プロパティ
        private AirSupremacy _FriendAirSupremacy = AirSupremacy.航空戦なし;

        public AirSupremacy FriendAirSupremacy
        {
            get
            { return this._FriendAirSupremacy; }
            set
            {
                if (this._FriendAirSupremacy == value)
                    return;
                this._FriendAirSupremacy = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region AirCombatResults変更通知プロパティ
        private AirCombatResult[] _AirCombatResults = new AirCombatResult[0];

        public AirCombatResult[] AirCombatResults
        {
            get
            { return this._AirCombatResults; }
            set
            {
                if (this._AirCombatResults.Equals(value))
                    return;
                this._AirCombatResults = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        #region DropShipName変更通知プロパティ
        private string _DropShipName;

        public string DropShipName
        {
            get
            { return this._DropShipName; }
            set
            {
                if (this._DropShipName == value)
                    return;
                this._DropShipName = value;
                this.RaisePropertyChanged();
            }
        }
        #endregion


        private int CurrentDeckId { get; set; }

        private BattleData()
        {
            var proxy = KanColleClient.Current.Proxy;

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/battle")
                .TryParse<battle_midnight_battle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_battle_midnight/sp_midnight")
                .TryParse<battle_midnight_sp_midnight>().Subscribe(x => this.Update(x.Data));

            proxy.api_req_combined_battle_airbattle
                .TryParse<combined_battle_airbattle>().Subscribe(x => this.Update(x.Data));

            proxy.api_req_combined_battle_battle
                .TryParse<combined_battle_battle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/battle_water")
                .TryParse<combined_battle_battle_water>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/midnight_battle")
                .TryParse<combined_battle_midnight_battle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/sp_midnight")
                .TryParse<combined_battle_sp_midnight>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/battle")
                .TryParse<practice_battle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_practice/midnight_battle")
                .TryParse<practice_midnight_battle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_sortie/airbattle")
                .TryParse<sortie_airbattle>().Subscribe(x => this.Update(x.Data));

            proxy.api_req_sortie_battle
                .TryParse<sortie_battle>().Subscribe(x => this.Update(x.Data));


            proxy.api_req_sortie_battleresult
                .TryParse<battle_result>().Subscribe(x => this.Update(x.Data));

            proxy.api_req_combined_battle_battleresult
                .TryParse<battle_result>().Subscribe(x => this.Update(x.Data));


            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/start")
                .TryParse<map_start_next>().Subscribe(x => this.UpdateFleetsByStartNext(x.Data, x.Request["api_deck_id"]));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_map/next")
                .TryParse<map_start_next>().Subscribe(x => this.UpdateFleetsByStartNext(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "kcsapi/api_req_sortie/ld_airbattle")
                .TryParse<sortie_airbattle>().Subscribe(x => this.Update(x.Data));

            proxy.ApiSessionSource.Where(x => x.Request.PathAndQuery == "/kcsapi/api_req_combined_battle/ld_airbattle")
                .TryParse<combined_battle_airbattle>().Subscribe(x => this.Update(x.Data));
        }

        #region Update From Battle SvData

        public void Update(battle_midnight_battle data)
        {
            this.Name = "通常 - 夜戦";

            this.UpdateData(data);
        }

        public void Update(battle_midnight_sp_midnight data)
        {
            this.Name = "通常 - 開幕夜戦";

            this.UpdateData(data);
        }

        public void Update(combined_battle_airbattle data)
        {
            this.Name = "連合艦隊 - 航空戦 - 昼戦";

            this.UpdateData(data);
        }

        public void Update(combined_battle_battle data)
        {
            this.Name = "連合艦隊 - 機動部隊 - 昼戦";

            this.UpdateData(data);
        }

        public void Update(combined_battle_battle_water data)
        {
            this.Name = "連合艦隊 - 水上部隊 - 昼戦";

            this.UpdateData(data);
        }

        public void Update(combined_battle_midnight_battle data)
        {
            this.Name = "連合艦隊 - 夜戦";

            this.UpdateData(data);
        }

        public void Update(combined_battle_sp_midnight data)
        {
            this.Name = "連合艦隊 - 開幕夜戦";

            this.UpdateData(data);
        }

        public void Update(practice_battle data)
        {
            this.Clear();

            this.Name = "演習 - 昼戦";

            this.UpdateData(data);
        }

        public void Update(practice_midnight_battle data)
        {
            this.Name = "演習 - 夜戦";

            this.UpdateData(data);
        }

        private void Update(sortie_airbattle data)
        {
            this.Name = "航空戦 - 昼戦";

            this.UpdateData(data);
        }

        private void Update(sortie_battle data)
        {
            this.Name = "通常 - 昼戦";

            this.UpdateData(data);
        }

        #endregion

        public void Update(battle_result data)
        {
            this.DropShipName = data.api_get_ship?.api_ship_name;

            switch (data.api_win_rank)
            {
                case "S":
                    this.BattleResult =
                        this.FirstFleet.Ships
                            .Concat(this.SecondFleet?.Ships ?? new ShipData[0])
                            .ToArray()
                            .GetHpLostPersent() > 0
                            ? BattleResult.勝利S
                            : BattleResult.完全勝利S;
                    break;
                case "A":
                    this.BattleResult = BattleResult.勝利A;
                    break;
                case "B":
                    this.BattleResult = BattleResult.戦術的勝利B;
                    break;
                case "C":
                    this.BattleResult = BattleResult.戦術的敗北C;
                    break;
                case "D":
                    this.BattleResult = BattleResult.敗北D;
                    break;
                case "E":
                    this.BattleResult = BattleResult.敗北E;
                    break;
            }
        }

        private void UpdateFleetsByStartNext(map_start_next startNext, string api_deck_id = null)
        {
            this.IsInBattle = false;

            this.Clear();

            if (api_deck_id != null) this.CurrentDeckId = int.Parse(api_deck_id);
            if (this.CurrentDeckId < 1) return;

            this.UpdateFriendFleets(this.CurrentDeckId);

            this.NextCell = new MapPoint(startNext);
        }


        private void UpdateFriendFleets(int deckID)
        {
            var organization = KanColleClient.Current.Homeport.Organization;
            this.FirstFleet = new FleetData(
                organization.Fleets[deckID].Ships.Select(s => new MembersShipData(s)).ToArray(),
                this.FirstFleet?.Formation ?? Formation.なし,
                organization.Fleets[deckID].Name,
                FleetType.First);
            this.SecondFleet = new FleetData(
                organization.Combined && deckID == 1
                    ? organization.Fleets[2].Ships.Select(s => new MembersShipData(s)).ToArray()
                    : new MembersShipData[0],
                this.SecondFleet?.Formation ?? Formation.なし,
                organization.Fleets[2].Name,
                FleetType.Second);
        }


        private void UpdateData<T>(T data) where T : ICommonBattleMembers, IFleetBattleInfo
        {
            this.IsInBattle = true;
            this.UpdatedTime = DateTimeOffset.Now;

            this.State = data is IPracticeData
                ? BattleState.Practice
                : BattleState.InSortie;

            var formation = data as IBattleFormationInfo;
            if (formation != null)
            {
                this.UpdateFleets(data);
                this.UpdateFormation(formation);
            }

            this.UpdateFleetsHPs(data);
            this.UpdateDamages(data);

            this.UpdateAirStage(data as IAirStageMembers);

            this.BattleResult = this.GetBattleResult();
        }

        private void UpdateFleets(ICommonBattleMembers data)
        {
            this.UpdateFriendFleets(data.api_deck_id);
            this.Enemies = new FleetData(
                data.ToMastersShipDataArray(),
                this.Enemies?.Formation ?? Formation.なし,
                this.Enemies?.Name ?? "",
                FleetType.Enemy);
        }

        private void UpdateFormation(IBattleFormationInfo data)
        {

            if (data.api_formation == null) return;

            this.BattleSituation = (BattleSituation)data.api_formation[2];
            if (this.FirstFleet != null) this.FirstFleet.Formation = (Formation)data.api_formation[0];
            if (this.Enemies != null) this.Enemies.Formation = (Formation)data.api_formation[1];
        }

        private void UpdateFleetsHPs(ICommonBattleMembers data)
        {
            this.FirstFleet.UpdateHPs(data.api_maxhps.GetFriendData(), data.api_nowhps.GetFriendData());
            this.Enemies.UpdateHPs(data.api_maxhps.GetEnemyData(), data.api_nowhps.GetEnemyData());

            var combined = data as ICombinedBattleMembers;
            if (combined != null)
                this.SecondFleet.UpdateHPs(combined.api_maxhps_combined.GetFriendData(), combined.api_nowhps_combined.GetFriendData());
        }

        private void UpdatePracticeDamages(IFleetBattleInfo data)
        {
            this.FirstFleet.CalcPracticeDamages(data.FirstFleetDamages);
            this.SecondFleet?.CalcPracticeDamages(data.SecondFleetDamages);
            this.Enemies.CalcPracticeDamages(data.EnemyDamages);
        }

        private void UpdateDamages(IFleetBattleInfo data)
        {
            if (data is IPracticeData)
            {
                this.UpdatePracticeDamages(data);
            }
            else
            {
                this.FirstFleet.CalcDamages(data.FirstFleetDamages);
                this.SecondFleet?.CalcDamages(data.SecondFleetDamages);
                this.Enemies.CalcDamages(data.EnemyDamages);
            }
        }

        private void UpdateAirStage(IAirStageMembers data)
        {
            if (data == null) return;

            this.FriendAirSupremacy = data.api_kouku.GetAirSupremacy();

            var combat = data as IAirBattleMembers;
            if (combat?.api_kouku2 != null)
            {
                this.AirCombatResults = combat.api_kouku.ToResult("1回目/")
                    .Concat(combat.api_kouku2.ToResult("2回目/")).ToArray();
            }
            else
            {
                this.AirCombatResults = data.api_kouku.ToResult();
            }
        }


        private void Clear()
        {
            this.UpdatedTime = DateTimeOffset.Now;
            this.Name = "";
            this.DropShipName = null;

            this.BattleSituation = BattleSituation.なし;
            this.FriendAirSupremacy = AirSupremacy.航空戦なし;
            this.AirCombatResults = new AirCombatResult[0];
            if (this.FirstFleet != null) this.FirstFleet.Formation = Formation.なし;
            this.Enemies = new FleetData();

            this.BattleResult = BattleResult.なし;
        }
    }
}
