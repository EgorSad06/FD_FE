using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FD_FE;

namespace FD_MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для DB_battle.xaml
    /// </summary>
    public partial class DB_battle : Page
    {
        public DB_battle()
        {
            InitializeComponent();

            Game.Cnct = new FD_Tools.Connection.Connection();
            Game.Cnct.is_host = true;
            Game.SetMode(1);
            Game.start_turn = true;
            Game.p_deck = new Deck(GameplayData.StartCards['t'].FindAll((card) => card.card_class.id == 'c'));
            Game.o_deck = new Deck();
            Game.p_deck.SetSqnc();

            _cur_battle = new FD_FE.Stages.Battle(MainBoardGrid, HandBoardGrid);

            B_ready.IsEnabled = Game.start_turn;

            _cur_battle.ScoreChanged += (p_score, o_score, turns) =>
            {
                TB_o_score.Text = o_score.ToString();
                TB_p_score.Text = p_score.ToString();
                TB_turns.Text = turns.ToString();
            };

            _cur_battle.StateChanged += (state) =>
            {
                State.Text = State_names[state];
                B_ready.IsEnabled = state == 1;
            };
            _cur_battle.BattleEnded += EndBattle;
            _cur_battle.Start();
        }
        private readonly FD_FE.Stages.Battle _cur_battle;
        private readonly Dictionary<int, string> State_names = new Dictionary<int, string>() {
            { 1, "Ход" },
            //{ 1, "Расположенте\nкарты" },
            { 2, "Назначение\nдействия" },
            { 3, "Ход\nсоперника" },
            { -1, "Победа" },
            { -2, "Ничья" },
            { -3, "Поражение" }
        };

        private void Ready_Click(object sender, RoutedEventArgs e)
        {
            //B_ready.IsEnabled = false;
            //_cur_battle.Ready();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            _cur_battle.ClearAct();
        }
        private async void EndBattle(short state)
        {
            App.CurrentStats.EnemiesKilled += _cur_battle.o_score;
            await Task.Delay(1000);

            if (Game.battle < Game.Mode.battles)
            {
                NavigationService.Navigate(new Uri("Pages/CardSelection.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                if (Game.p_global_score > Game.o_global_score)
                {
                    App.CurrentStats.Wins++;
                    MessageBox.Show("Вы победили в этой игре!");
                }
                else if (Game.p_global_score == Game.o_global_score)
                {
                    MessageBox.Show("В этой игре ничья");
                }
                else
                {
                    App.CurrentStats.Losses++;
                    MessageBox.Show("Вы проиграли в этой игре");
                }
                NavigationService.Navigate(new Uri("Pages/StartGame.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
        }
    }
}