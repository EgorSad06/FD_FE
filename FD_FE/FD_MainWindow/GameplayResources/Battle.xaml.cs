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

namespace FD_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для Battle.xaml
    /// </summary>
    public partial class Battle : Page
    {
        public Battle()
        {
            InitializeComponent();
            Start();
        }

        public static Board Main_board = new Board(Game.Mode.board_width, Game.Mode.board_height);
        public static Board Hand_board = new Board(1, 7);

        public static BoardCard slct_card = null;
        public static byte[] act_sqnc = null;
        public static short p_score = 0;
        public static short o_score = 0;
// начало игры
        private async void Start()
        {
            if (Game.is_host) // перемешивание колоды
            {
                int seed = Game.p_deck.SetSqnc();
                Game.SendData(BitConverter.GetBytes(seed));
                Game.o_deck.SetSqnc(seed+4013);
            }
            else
            {
                int seed = BitConverter.ToInt32(await Game.ReceiveData(4), 0);
                Game.p_deck.SetSqnc(seed+4013);
                Game.o_deck.SetSqnc(seed);
            }

            for (int i = 0; i < 3 && Game.p_deck.SqncEnd(); i++) // помещение в колоду 4 стартовых карт
            {
                Hand_board.SetBoardCard(Game.p_deck.MoveToHand(), i);
            }
            for (int i = 0; i < 3 && Game.o_deck.SqncEnd(); i++)
            {
                Game.o_deck.MoveToHand();
            }

            Game.Draw(Hand_board, HandBoardGrid);
            Game.Draw(Main_board, MainBoardGrid, (float) ((Game.Mode.board_height == 3) ? 1.8 : 1.6));

            if (Game.start_turn) Turn();
            else Wait();
        }

// события хода
        private void OnCardClicked(UCCard UCcard, BoardCard card)
        {
            if (HandBoardGrid.Children.Contains(UCcard))
            {
                UCSlot.SlotSelected += OnSlotClicked;
            }
            else
            {
                UCSlot.SlotSelected -= OnSlotClicked;
            }

        }
        private void OnSlotClicked(short i)
        {
            Main_board.SetBoardCard(slct_card, i);
        }

// управление этапами игры
        private void Turn()
        {
            Game.p_deck.MoveToHand();
            UCCard.CardSelected += OnCardClicked;
            

        }
        private async void Wait()
        {
            UCCard.CardSelected -= OnCardClicked;
            Game.p_deck.MoveToHand();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void Ready_Click(object sender, RoutedEventArgs e)
        {

        }

        private void End_Battle()
        {
            if (p_score > o_score)
            {
                MessageBox.Show("Вы победили!");
                Game.o_global_score++;
                Game.start_turn = false;
            }
            else if (p_score == o_score)
            {
                MessageBox.Show("Ничья");
                Game.start_turn = !Game.start_turn;
            }
            else
            {
                MessageBox.Show("Вы проиграли");
                Game.p_global_score++;
                Game.start_turn = true;
            }
            if (++Game.battle < Game.Mode.battles)
            {
                NavigationService.Navigate(new Uri("GameplayResources/CardSelection.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
            else
            {
                Game.battle = 0;
                Game.slct_cards = new Deck();
                Game.p_deck = new Deck();
                Game.o_deck = new Deck();
                act_sqnc = null;
                p_score = 0;
                o_score = 0;
        NavigationService.Navigate(new Uri("GameplayResources/GameStart.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
        }
    }
}
