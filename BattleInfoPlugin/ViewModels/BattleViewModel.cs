using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using BattleInfoPlugin.Models;
using BattleInfoPlugin.Models.Notifiers;
using BattleInfoPlugin.Models.Settings;
using Livet;
using Livet.EventListeners;
using Livet.Messaging;
using MetroTrilithon.Lifetime;
using MetroTrilithon.Mvvm;

namespace BattleInfoPlugin.ViewModels
{
    public class BattleViewModel : ViewModel
    {
        public static BattleViewModel Current { get; } = new BattleViewModel();

        public BattleData Battle { get; } = BattleData.Current;

        #region IsShowLandBaseAirStage

        public bool IsShowLandBaseAirStage
        {
            get { return PluginSettings.BattleData.IsShowLandBaseAirStage.Value; }
            set
            {
                if (PluginSettings.BattleData.IsShowLandBaseAirStage.Value != value)
                {
                    PluginSettings.BattleData.IsShowLandBaseAirStage.Value = value;
                    this.UpdateLandBaseVisibility(value);
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        public string BattleResultRank
            => this.Battle.BattleResult != Models.BattleResultRank.なし
                ? this.Battle.BattleResult.ToString()
                : "";

        public string UpdatedTime
            => this.Battle != null && this.Battle.UpdatedTime != default(DateTimeOffset)
                ? this.Battle.UpdatedTime.ToString("yyyy/MM/dd HH:mm:ss")
                : "No Data";

        public string BattleSituation
            => this.Battle != null && this.Battle.BattleSituation != Models.BattleSituation.なし
                ? this.Battle.BattleSituation.ToString()
                : "";

        public string FriendAirSupremacy
            => this.Battle.FriendAirSupremacy != AirSupremacy.航空戦なし
                ? this.Battle.FriendAirSupremacy.ToString()
                : "";

        #region FriendFleet

        private BattleFleetViewModel _FriendFleet;

        public BattleFleetViewModel FriendFleet
        {
            get { return this._FriendFleet; }
            set
            {
                if (this._FriendFleet != value)
                {
                    this._FriendFleet = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region EnemyFleet

        private BattleFleetViewModel _EnemyFleet;

        public BattleFleetViewModel EnemyFleet
        {
            get { return this._EnemyFleet; }
            set
            {
                if (this._EnemyFleet != value)
                {
                    this._EnemyFleet = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region NextCellInfo

        private NextCellInfoViewModel _nextCellInfo = new NextCellInfoViewModel { IsInSortie = false };

        public NextCellInfoViewModel NextCellInfo
        {
            get { return this._nextCellInfo; }
            set
            {
                if (this._nextCellInfo != value)
                {
                    this._nextCellInfo = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region LandBaseVisibility

        private Visibility _LandBaseVisibility;

        public Visibility LandBaseVisibility
        {
            get { return this._LandBaseVisibility; }
            set
            {
                if (this._LandBaseVisibility != value)
                {
                    this._LandBaseVisibility = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region FriendInjectionAirCombatResults

        private AirCombatResultViewModel[] _FriendInjectionAirCombatResults;

        public AirCombatResultViewModel[] FriendInjectionAirCombatResults
        {
            get { return this._FriendInjectionAirCombatResults; }
            set
            {
                if (this._FriendInjectionAirCombatResults != value)
                {
                    this._FriendInjectionAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region EnemyInjectionAirCombatResults

        private AirCombatResultViewModel[] _EnemyInjectionAirCombatResults;

        public AirCombatResultViewModel[] EnemyInjectionAirCombatResults
        {
            get { return this._EnemyInjectionAirCombatResults; }
            set
            {
                if (this._EnemyInjectionAirCombatResults != value)
                {
                    this._EnemyInjectionAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region FriendLandBaseAirCombatResults

        private LandBaseAirCombatResultViewModel[] _FriendLandBaseAirCombatResults;

        public LandBaseAirCombatResultViewModel[] FriendLandBaseAirCombatResults
        {
            get { return this._FriendLandBaseAirCombatResults; }
            set
            {
                if (this._FriendLandBaseAirCombatResults != value)
                {
                    this._FriendLandBaseAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region EnemyLandBaseAirCombatResults

        private LandBaseAirCombatResultViewModel[] _EnemyLandBaseAirCombatResults;

        public LandBaseAirCombatResultViewModel[] EnemyLandBaseAirCombatResults
        {
            get { return this._EnemyLandBaseAirCombatResults; }
            set
            {
                if (this._EnemyLandBaseAirCombatResults != value)
                {
                    this._EnemyLandBaseAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region FriendAirCombatResults

        private AirCombatResultViewModel[] _FriendAirCombatResults;

        public AirCombatResultViewModel[] FriendAirCombatResults
        {
            get { return this._FriendAirCombatResults; }
            set
            {
                if (this._FriendAirCombatResults != value)
                {
                    this._FriendAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion


        #region EnemyAirCombatResults

        private AirCombatResultViewModel[] _EnemyAirCombatResults;

        public AirCombatResultViewModel[] EnemyAirCombatResults
        {
            get { return this._EnemyAirCombatResults; }
            set
            {
                if (this._EnemyAirCombatResults != value)
                {
                    this._EnemyAirCombatResults = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        #endregion

        private BattleViewModel()
        {
            this.FriendFleet = new BattleFleetViewModel(this.Battle.FriendFleet, "自艦隊");
            this.FriendFleet.ObserveUpdate();
            this.EnemyFleet = new BattleFleetViewModel(this.Battle.EnemyFleet, "敵艦隊");
            this.EnemyFleet.ObserveUpdate();

            Dictionary<string, string> MapCell = new Dictionary<string, string>() {
                /* 1-1 */
                { "1-1-1", "A" },
                { "1-1-2", "B" },
                { "1-1-3", "C" },
                /* 1-2 */
                { "1-2-1", "A" },
                { "1-2-2", "B" },
                { "1-2-3", "C" },
                { "1-2-4", "D" },
                /* 1-3 */
                { "1-3-1", "A" },
                { "1-3-2", "B" },
                { "1-3-3", "D" },
                { "1-3-4", "C" },
                { "1-3-5", "E" },
                { "1-3-6", "F" },
                { "1-3-7", "G" },
                { "1-3-8", "C" },
                { "1-3-9", "F" },
                /* 1-4 */
                { "1-4-1", "A" },
                { "1-4-2", "B" },
                { "1-4-3", "G" },
                { "1-4-4", "C" },
                { "1-4-5", "H" },
                { "1-4-6", "D" },
                { "1-4-7", "E" },
                { "1-4-8", "I" },
                { "1-4-9", "J" },
                { "1-4-10", "F" },
                { "1-4-11", "C" },
                { "1-4-12", "F" },
                { "1-4-13", "F" },
                /* 1-5 */
                { "1-5-1", "A" },
                { "1-5-2", "B" },
                { "1-5-3", "D" },
                { "1-5-4", "C" },
                { "1-5-5", "E" },
                { "1-5-6", "F" },
                { "1-5-7", "H" },
                { "1-5-8", "G" },
                { "1-5-10", "I" },
                /* 1-6 */
                { "1-6-1", "A" },
                { "1-6-2", "C" },
                { "1-6-3", "E" },
                { "1-6-4", "G" },
                { "1-6-5", "H" },
                { "1-6-6", "K" },
                { "1-6-7", "M" },
                { "1-6-8", "L" },
                { "1-6-9", "J" },
                { "1-6-10", "I" },
                { "1-6-11", "D" },
                { "1-6-12", "F" },
                { "1-6-13", "B" },
                { "1-6-14", "N" },
                { "1-6-15", "K" },
                { "1-6-16", "D" },
                { "1-6-17", "N" },
                /* 2-1 */
                { "2-1-1", "A" },
                { "2-1-2", "B" },
                { "2-1-3", "D" },
                { "2-1-4", "C" },
                { "2-1-5", "F" },
                { "2-1-6", "E" },
                /* 2-2 */
                { "2-2-1", "A" },
                { "2-2-2", "E" },
                { "2-2-3", "B" },
                { "2-2-4", "G" },
                { "2-2-5", "C" },
                { "2-2-6", "D" },
                { "2-2-7", "F" },
                { "2-2-8", "E" },
                /* 2-3 */
                { "2-3-1", "C" },
                { "2-3-2", "A" },
                { "2-3-3", "H" },
                { "2-3-4", "D" },
                { "2-3-5", "B" },
                { "2-3-6", "E" },
                { "2-3-7", "I" },
                { "2-3-8", "J" },
                { "2-3-9", "K" },
                { "2-3-10", "F" },
                { "2-3-11", "G" },
                { "2-3-12", "E" },
                /* 2-4 */
                { "2-4-1", "A" },
                { "2-4-2", "K" },
                { "2-4-3", "F" },
                { "2-4-4", "B" },
                { "2-4-5", "C" },
                { "2-4-6", "D" },
                { "2-4-7", "E" },
                { "2-4-8", "L" },
                { "2-4-9", "M" },
                { "2-4-10", "N" },
                { "2-4-11", "G" },
                { "2-4-12", "I" },
                { "2-4-13", "H" },
                { "2-4-14", "O" },
                { "2-4-15", "P" },
                { "2-4-16", "J" },
                { "2-4-17", "J" },
                { "2-4-18", "J" },
                { "2-4-19", "G" },
                /* 2-5 */
                { "2-5-1", "A" },
                { "2-5-2", "B" },
                { "2-5-3", "C" },
                { "2-5-4", "F" },
                { "2-5-5", "D" },
                { "2-5-6", "E" },
                { "2-5-7", "G" },
                { "2-5-8", "H" },
                { "2-5-9", "I" },
                { "2-5-10", "J" },
                { "2-5-12", "L" },
                { "2-5-13", "H" },
                { "2-5-14", "H" },
                { "2-5-15", "L" },
                /* 3-1 */
                { "3-1-1", "D" },
                { "3-1-2", "A" },
                { "3-1-3", "F" },
                { "3-1-4", "B" },
                { "3-1-5", "C" },
                { "3-1-6", "E" },
                /* 3-2 */
                { "3-2-1", "E" },
                { "3-2-2", "A" },
                { "3-2-3", "D" },
                { "3-2-4", "F" },
                { "3-2-5", "B" },
                { "3-2-6", "C" },
                { "3-2-7", "G" },
                { "3-2-8", "H" },
                { "3-2-9", "B" },
                { "3-2-10", "F"},
                /* 3-3 */
                { "3-3-1", "A" },
                { "3-3-2", "B" },
                { "3-3-3", "E" },
                { "3-3-4", "H" },
                { "3-3-5", "C" },
                { "3-3-6", "F" },
                { "3-3-7", "I" },
                { "3-3-8", "D" },
                { "3-3-9", "K" },
                { "3-3-10", "J" },
                { "3-3-11", "G" },
                { "3-3-12", "I" },
                { "3-3-13", "G" },
                /* 3-4 */
                { "3-4-1", "A" },
                { "3-4-2", "J" },
                { "3-4-3", "K" },
                { "3-4-4", "B" },
                { "3-4-5", "F" },
                { "3-4-6", "L" },
                { "3-4-7", "G" },
                { "3-4-8", "M" },
                { "3-4-9", "C" },
                { "3-4-10", "H" },
                { "3-4-11", "N" },
                { "3-4-12", "D" },
                { "3-4-13", "I" },
                { "3-4-14", "O" },
                { "3-4-15", "E" },
                { "3-4-16", "F" },
                { "3-4-17", "L" },
                { "3-4-18", "H" },
                { "3-4-20", "E" },
                /* 3-5 */
                { "3-5-1", "A" },
                { "3-5-3", "D" },
                { "3-5-4", "C" },
                { "3-5-6", "B" },
                { "3-5-7", "G" },
                { "3-5-8", "F" },
                { "3-5-9", "I" },
                { "3-5-10", "J" },
                { "3-5-11", "K" },
                { "3-5-12", "E" },
                { "3-5-13", "B" },
                { "3-5-14", "F" },
                { "3-5-15", "K" },
                /* 4-1 */
                { "4-1-1", "E" },
                { "4-1-2", "A" },
                { "4-1-3", "H" },
                { "4-1-4", "F" },
                { "4-1-5", "I" },
                { "4-1-6", "B" },
                { "4-1-7", "G" },
                { "4-1-8", "C" },
                { "4-1-9", "D" },
                { "4-1-10", "I" },
                { "4-1-11", "I" },
                { "4-1-12", "D" },
                /* 4-2 */
                { "4-2-1", "F" },
                { "4-2-2", "A" },
                { "4-2-3", "B" },
                { "4-2-4", "G" },
                { "4-2-5", "E" },
                { "4-2-6", "C" },
                { "4-2-7", "H" },
                { "4-2-8", "I" },
                { "4-2-9", "D" },
                { "4-2-10", "E" },
                { "4-2-11", "H" },
                { "4-2-12", "H" },
                { "4-2-13", "D" },
                /* 4-3 */
                { "4-3-1", "J" },
                { "4-3-2", "A" },
                { "4-3-3", "B" },
                { "4-3-4", "F" },
                { "4-3-5", "K" },
                { "4-3-6", "C" },
                { "4-3-7", "D" },
                { "4-3-8", "G" },
                { "4-3-9", "H" },
                { "4-3-10", "L" },
                { "4-3-11", "E" },
                { "4-3-12", "M" },
                { "4-3-13", "I" },
                { "4-3-14", "F" },
                { "4-3-15", "F" },
                { "4-3-16", "K" },
                { "4-3-17", "D" },
                { "4-3-18", "D" },
                { "4-3-19", "G" },
                { "4-3-20", "G" },
                { "4-3-21", "L" },
                /* 4-4 */
                { "4-4-1", "A" },
                { "4-4-2", "B" },
                { "4-4-3", "I" },
                { "4-4-4", "F" },
                { "4-4-5", "C" },
                { "4-4-6", "D" },
                { "4-4-7", "G" },
                { "4-4-8", "J" },
                { "4-4-9", "E" },
                { "4-4-10", "H" },
                { "4-4-11", "C" },
                { "4-4-12", "G" },
                { "4-4-13", "G" },
                { "4-4-14", "J" },
                /* 4-5 */
                { "4-5-1", "A" },
                { "4-5-2", "B" },
                { "4-5-3", "D" },
                { "4-5-4", "C" },
                { "4-5-5", "E" },
                { "4-5-6", "F" },
                { "4-5-7", "G" },
                { "4-5-8", "H" },
                { "4-5-9", "I" },
                { "4-5-10", "J" },
                { "4-5-11", "K" },
                { "4-5-12", "L" },
                { "4-5-13", "M" },
                { "4-5-14", "C" },
                { "4-5-15", "F" },
                { "4-5-16", "F" },
                { "4-5-17", "H" },
                { "4-5-18", "J" },
                { "4-5-19", "M" },
                /* 5-1 */
                { "5-1-1", "B" },
                { "5-1-2", "A" },
                { "5-1-3", "D" },
                { "5-1-4", "C" },
                { "5-1-5", "F" },
                { "5-1-6", "E" },
                { "5-1-7", "H" },
                { "5-1-8", "G" },
                { "5-1-9", "I" },
                { "5-1-10", "F" },
                { "5-1-11", "E" },
                { "5-1-12", "H" },
                /* 5-2 */
                { "5-2-1", "A" },
                { "5-2-2", "B" },
                { "5-2-3", "F" },
                { "5-2-4", "G" },
                { "5-2-5", "E" },
                { "5-2-6", "C" },
                { "5-2-7", "L" },
                { "5-2-8", "H" },
                { "5-2-9", "J" },
                { "5-2-10", "D" },
                { "5-2-11", "G" },
                { "5-2-12", "H" },
                { "5-2-13", "J" },
                { "5-2-14", "D" },
                /* 5-3 */
                { "5-3-1", "A" },
                { "5-3-2", "B" },
                { "5-3-3", "C" },
                { "5-3-4", "D" },
                { "5-3-5", "E" },
                { "5-3-6", "F" },
                { "5-3-7", "G" },
                { "5-3-8", "H" },
                { "5-3-9", "I" },
                { "5-3-10", "J" },
                { "5-3-11", "K" },
                { "5-3-12", "D" },
                { "5-3-13", "I" },
                { "5-3-14", "I" },
                /* 5-4 */
                { "5-4-1", "A" },
                { "5-4-2", "B" },
                { "5-4-3", "C" },
                { "5-4-4", "F" },
                { "5-4-5", "G" },
                { "5-4-6", "I" },
                { "5-4-7", "E" },
                { "5-4-8", "D" },
                { "5-4-9", "L" },
                { "5-4-10", "N" },
                { "5-4-11", "M" },
                { "5-4-12", "H" },
                { "5-4-13", "K" },
                { "5-4-14", "J" },
                { "5-4-15", "O" },
                { "5-4-16", "I" },
                { "5-4-17", "H" },
                { "5-4-18", "M" },
                { "5-4-19", "O" },
                { "5-4-20", "O" },
                /* 5-5 */
                { "5-5-1", "B" },
                { "5-5-2", "A" },
                { "5-5-3", "F" },
                { "5-5-4", "C" },
                { "5-5-5", "D" },
                { "5-5-6", "K" },
                { "5-5-7", "M" },
                { "5-5-8", "J" },
                { "5-5-9", "I" },
                { "5-5-10", "G" },
                { "5-5-11", "H" },
                { "5-5-12", "E" },
                { "5-5-13", "L" },
                { "5-5-14", "N" },
                { "5-5-15", "G" },
                { "5-5-16", "H" },
                { "5-5-17", "E" },
                { "5-5-18", "N" },
                /* 6-1 */
                { "6-1-1", "G" },
                { "6-1-2", "A" },
                { "6-1-3", "B" },
                { "6-1-4", "C" },
                { "6-1-5", "D" },
                { "6-1-6", "E" },
                { "6-1-7", "H" },
                { "6-1-8", "F" },
                { "6-1-9", "I" },
                { "6-1-10", "J" },
                { "6-1-11", "K" },
                { "6-1-12", "D" },
                { "6-1-13", "D" },
                /* 6-2 */
                { "6-2-1", "A" },
                { "6-2-2", "B" },
                { "6-2-3", "C" },
                { "6-2-4", "D" },
                { "6-2-5", "E" },
                { "6-2-6", "F" },
                { "6-2-7", "G" },
                { "6-2-8", "J" },
                { "6-2-9", "H" },
                { "6-2-10", "I" },
                { "6-2-11", "K" },
                { "6-2-12", "B" },
                { "6-2-13", "D" },
                { "6-2-14", "E" },
                { "6-2-15", "J" },
                { "6-2-16", "H" },
                { "6-2-17", "K" },
                { "6-2-18", "K" },
                /* 6-3 */
                { "6-3-1", "A" },
                { "6-3-2", "B" },
                { "6-3-3", "C" },
                { "6-3-4", "D" },
                { "6-3-5", "E" },
                { "6-3-6", "F" },
                { "6-3-7", "G" },
                { "6-3-8", "H" },
                { "6-3-9", "I" },
                { "6-3-10", "J" },
                { "6-3-11", "E" },
                { "6-3-12", "H" },
                /* 6-4 */
                { "6-4-1", "A" },
                { "6-4-2", "B" },
                { "6-4-3", "C" },
                { "6-4-4", "D" },
                { "6-4-5", "E" },
                { "6-4-6", "F" },
                { "6-4-7", "G" },
                { "6-4-8", "H" },
                { "6-4-9", "I" },
                { "6-4-10", "J" },
                { "6-4-11", "K" },
                { "6-4-12", "L" },
                { "6-4-13", "M" },
                { "6-4-14", "N" },
                { "6-4-15", "D" },
                { "6-4-16", "D" },
                { "6-4-17", "D" },
                { "6-4-18", "J" },
                { "6-4-19", "I" },
                { "6-4-20", "N" },
                { "6-4-21", "N" },
                { "6-4-22", "2" },
                /* 6-5 */
                { "6-5-1", "A" },
                { "6-5-2", "B" },
                { "6-5-3", "C" },
                { "6-5-4", "D" },
                { "6-5-5", "E" },
                { "6-5-6", "F" },
                { "6-5-7", "G" },
                { "6-5-9", "I" },
                { "6-5-10", "J" },
                { "6-5-13", "M" },
                { "6-5-15", "G" },
                { "6-5-16", "H" },
                { "6-5-17", "I" },
                { "6-5-18", "M" }
            };

            this.CompositeDisposable.Add(new PropertyChangedEventListener(this.Battle)
            {
                {
                    () => this.Battle.BattleResult,
                    (_, __) => this.RaisePropertyChanged(() => this.BattleResultRank)
                },
                {
                    () => this.Battle.UpdatedTime,
                    (_, __) => this.RaisePropertyChanged(() => this.UpdatedTime)
                },
                {
                    () => this.Battle.BattleSituation,
                    (_, __) => this.RaisePropertyChanged(() => this.BattleSituation)
                },
                {
                    () => this.Battle.FriendAirSupremacy,
                    (_, __) => this.RaisePropertyChanged(() => this.FriendAirSupremacy)
                },
                {
                    () => this.Battle.InjectionAirCombatResults,
                    (_, __) =>
                    {
                        this.FriendInjectionAirCombatResults = this.Battle.InjectionAirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Friend)).ToArray();
                        this.EnemyInjectionAirCombatResults = this.Battle.InjectionAirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Enemy)).ToArray();
                    }
                },
                {
                    () => this.Battle.LandBaseAirCombatResults,
                    (_, __) =>
                    {
                        var landbase = this.Battle.LandBaseAirCombatResults;
                        this.UpdateLandBaseVisibility(landbase.Length > 0);
                        this.FriendLandBaseAirCombatResults = landbase.Select(x => new LandBaseAirCombatResultViewModel(x, FleetType.Friend)).ToArray();
                        this.EnemyLandBaseAirCombatResults = landbase.Select(x => new LandBaseAirCombatResultViewModel(x, FleetType.Enemy)).ToArray();
                    }
                },
                {
                    () => this.Battle.AirCombatResults,
                    (_, __) =>
                    {
                        this.FriendAirCombatResults = this.Battle.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Friend)).ToArray();
                        this.EnemyAirCombatResults = this.Battle.AirCombatResults.Select(x => new AirCombatResultViewModel(x, FleetType.Enemy)).ToArray();
                    }
                },
                {
                    () => this.Battle.NextCell,
                    (_, __) =>
                    {
                        var nextCell = this.Battle.NextCell;

                        var getLostItems = new List<GetLostItemViewModel>();
                        getLostItems.AddRange(nextCell.GetLostItems.Select(item => new GetLostItemViewModel(item)));

                        this.NextCellInfo = new NextCellInfoViewModel
                        {
                            MapId = nextCell.MapId.ToString(),
                            //Id = ((char) (nextCell.Id - 1 + 'A')).ToString(),
                            Id = MapCell.ContainsKey(nextCell.MapId.ToString() + '-' + nextCell.Id.ToString()) 
                            ? MapCell[nextCell.MapId.ToString() + '-' + nextCell.Id.ToString()] 
                            : nextCell.Id.ToString(),
                            CellType = nextCell.Type,
                            IsInSortie = true,
                            GetLostItems = getLostItems.ToArray(),
                        };
                    }
                },
                {
                    () => this.Battle.State,
                    (_, __) => this.NextCellInfo = new NextCellInfoViewModel {IsInSortie = false}
                }
            });
        }

        private void UpdateLandBaseVisibility(bool visible)
        {
            this.LandBaseVisibility = visible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
