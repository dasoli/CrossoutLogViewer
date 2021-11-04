using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using CrossoutLogView.Database.Data;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.Statistics;
using static CrossoutLogView.Common.Strings;

namespace CrossoutLogView.GUI.Models
{
    public sealed class GameModel : CollectionViewModelBase
    {
        private PlayerModel _MVP;

        private List<PlayerModel> _players;

        private PlayerModel _RedMVP;

        private IEnumerable<WeaponModel> _weapons;

        public GameModel()
        {
            Game = new Game();
        }

        public GameModel(Game game)
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));
            UpdateCollectionsSafe();
        }

        public Game Game { get; }

        public PlayerModel MVP
        {
            get
            {
                if (_MVP == null && Game.MVP != -1)
                    Set(ref _MVP, Players.FirstOrDefault(x => x.Player.PlayerIndex == Game.MVP));
                return _MVP;
            }
        }

        public string MVPName => MVP == null ? string.Empty : App.GetControlResource("Game_Mvp") + MVP.Player.Name;

        public PlayerModel RedMVP
        {
            get
            {
                if (_RedMVP == null && Game.RedMVP != -1)
                    Set(ref _RedMVP, Players.FirstOrDefault(x => x.Player.PlayerIndex == Game.RedMVP));
                return _RedMVP;
            }
        }

        public string RedMVPName =>
            RedMVP == null ? string.Empty : App.GetControlResource("Game_RedMvp") + RedMVP.Player.Name;

        public string Team1String =>
            Game.MVP == -1 ? App.GetControlResource("Game_Team1") : App.GetControlResource("Game_TeamWin");

        public string Team2String =>
            Game.MVP == -1 ? App.GetControlResource("Game_Team2") : App.GetControlResource("Game_TeamLoose");

        public Visibility MVPVisible => Game.MVP != -1 ? Visibility.Visible : Visibility.Hidden;

        public Visibility RedMVPVisible => Game.RedMVP != -1 ? Visibility.Visible : Visibility.Hidden;

        public Visibility UnfinishedVisible => Game.MVP == -1 ? Visibility.Visible : Visibility.Hidden;

        public string Duration => App.GetControlResource("Game_Duration") + TimeSpanStringFactory(Start - End);

        public List<PlayerModel> Players
        {
            get => _players;
            private set => Set(ref _players, value);
        }

        public IEnumerable<WeaponModel> Weapons
        {
            get => _weapons;
            private set => Set(ref _weapons, value);
        }

        public DateTime Start => Game.Start;

        public DateTime End => Game.End;

        public GameMode Mode => Game.Mode;

        public string Mission => Game.Mission;

        public Map Map => Game.Map;

        public byte WinningTeam => Game.WinningTeam;

        public List<Round> Rounds => Game.Rounds;

        public bool Won => _players.FirstOrDefault(x => x.UserID == Settings.Current.MyUserID).Won;
        public bool Unfinished => Game.MVP == -1;

        protected override void UpdateCollections()
        {
            if (Game == null) return;
            Players = Game.Players.Select(x => new PlayerModel(this, x)).ToList();
            Weapons = Game.Weapons.Select(x => new WeaponModel(this, x)).ToList();
            OnPropertyChanged(nameof(Rounds));
        }
    }
}