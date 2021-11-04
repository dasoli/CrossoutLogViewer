using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using CrossoutLogView.Common;
using CrossoutLogView.Database.Data;
using CrossoutLogView.GUI.Core;
using CrossoutLogView.GUI.Helpers;
using CrossoutLogView.GUI.Models;
using CrossoutLogView.GUI.WindowsAuxilary;
using NLog;

namespace CrossoutLogView.GUI.Navigation
{
    /// <summary>
    ///     Interaction logic for GamePage.xaml
    /// </summary>
    public partial class GamePage : ILogging
    {
        private readonly GameModel gameModel;
        private readonly NavigationWindow nav;
        private bool isRoundKillerOver, isRoundVictimOver, isRoundAssistantOver;

        public GamePage(NavigationWindow nav, GameModel gameViewModel)
        {
            if (gameViewModel is null)
                throw new ArgumentNullException(nameof(gameViewModel));
            this.nav = nav ?? throw new ArgumentNullException(nameof(nav));
            DataProvider.CompleteGame(gameViewModel.Game);
            gameViewModel.UpdateCollectionsSafe();
            InitializeComponent();
            DataContext = gameModel = gameViewModel;
            TreeViewRounds.ItemsSource = gameViewModel.Rounds.Select(x => new RoundModel(gameViewModel, x));
            gameViewModel.Players.Sort(new PlayerModelScoreDescending());
            ListBoxWon.ItemsSource = gameViewModel.Players.Where(x => gameViewModel.WinningTeam == x.Player.Team);
            ListBoxLost.ItemsSource = gameViewModel.Players.Where(x => gameViewModel.WinningTeam != x.Player.Team);
            MapImage.Source = ImageHelper.GetMapImage(gameModel.Map.Name);
        }

        private void RoundKillEnter(object sender, MouseEventArgs e)
        {
            isRoundKillerOver = true;
        }

        private void RoundKillLeave(object sender, MouseEventArgs e)
        {
            isRoundKillerOver = false;
        }

        private void RoundVictimEnter(object sender, MouseEventArgs e)
        {
            isRoundVictimOver = true;
        }

        private void RoundVictimLeave(object sender, MouseEventArgs e)
        {
            isRoundVictimOver = false;
        }

        private void RoundAssistantEnter(object sender, MouseEventArgs e)
        {
            isRoundAssistantOver = true;
        }

        private void RoundAssistantLeave(object sender, MouseEventArgs e)
        {
            isRoundAssistantOver = false;
        }

        private void ScoreOpenPlayer(object sender, MouseButtonEventArgs e)
        {
            var clickedItem = DataGridHelper.GetSourceElement<ListBoxItem>(e.OriginalSource);
            if (clickedItem != null && clickedItem.DataContext is PlayerModel player)
            {
                nav.Navigate(new PlayerPage(nav, player));
                e.Handled = true;
            }
        }

        private void RoundOpenPlayer(object sender, MouseButtonEventArgs e)
        {
            PlayerModel targetPlayer = null;
            if (TreeViewRounds.SelectedItem is KillModel kv)
            {
                if (isRoundKillerOver) targetPlayer = PlayerByName(kv.Killer);
                else if (isRoundVictimOver) targetPlayer = PlayerByName(kv.Victim);
                e.Handled = true;
            }
            else if (TreeViewRounds.SelectedItem is AssistModel av)
            {
                if (isRoundAssistantOver) targetPlayer = PlayerByName(av.Assistant);
                e.Handled = true;
            }

            if (targetPlayer != null) nav.Navigate(new PlayerPage(nav, targetPlayer));
        }

        private void OpenMVP(object sender, MouseButtonEventArgs e)
        {
            if (gameModel.MVP != null)
            {
                nav.Navigate(new PlayerPage(nav, gameModel.MVP));
                e.Handled = true;
            }
        }

        private void OpenRedMVP(object sender, MouseButtonEventArgs e)
        {
            if (gameModel.RedMVP != null)
            {
                nav.Navigate(new PlayerPage(nav, gameModel.RedMVP));
                e.Handled = true;
            }
        }

        private PlayerModel PlayerByName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return gameModel.Players.FirstOrDefault(x => Strings.NameEquals(x.Name, name));
        }

        #region ILogging support

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        Logger ILogging.Logger { get; } = logger;

        #endregion
    }
}