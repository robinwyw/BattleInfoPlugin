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
            this.FriendFleet = new BattleFleetViewModel(this.Battle.FriendFleet, "第一艦隊");
            this.FriendFleet.ObserveUpdate();
            this.EnemyFleet = new BattleFleetViewModel(this.Battle.EnemyFleet, "敵艦隊");
            this.EnemyFleet.ObserveUpdate();

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
                            Id = ((char) (nextCell.Id - 1 + 'A')).ToString(),
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
