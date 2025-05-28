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
        private void Start()
        {
            if (Game.is_host) // перемешивание колоды
            {
                Game.p_seed += 4013;
                Game.o_seed -= 4013;
                Game.p_deck.SetSqnc(Game.p_seed);
                Game.o_deck.SetSqnc(Game.o_seed);
            }
            else
            {
                Game.p_seed -= 4013;
                Game.o_seed += 4013;
                Game.p_deck.SetSqnc(Game.p_seed);
                Game.o_deck.SetSqnc(Game.o_seed);
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

            Main_board.BoardChanged += (Board sender, int i) => { Game.Update(sender, i, MainBoardGrid, (float)((Game.Mode.board_height == 3) ? 1.8 : 1.6)); };
            Hand_board.BoardChanged += (Board sender, int i) => { Game.Update(sender, i, HandBoardGrid); };

            B_ready.IsEnabled = Game.start_turn;
            if (Game.start_turn) Turn();
            else Wait();
        }

// события хода
        private void OnCardSelected(UCCard sender, BoardCard card)
        {
            slct_card = sender.BoardCard;
            if (HandBoardGrid.Children.Contains(sender))
            {
                UCSlot.SlotSelected += OnSlotSelected;
            }
            else
            {
                UCSlot.SlotSelected -= OnSlotSelected;
            }

        }
        private void OnSlotSelected(short i)
        {
            Main_board.SetBoardCard(Hand_board.RemBoardCard(slct_card.board_i), i);
            UCSlot.SlotSelected -= OnSlotSelected;
        }

// управление этапами игры
        private void Turn()
        {
            if (Game.p_deck.hand_cards.Count<7 && Game.p_deck.SqncEnd()) Hand_board.AddBoardCard(Game.p_deck.MoveToHand());
            UCCard.CardSelected += OnCardSelected;
            

        }
        private async void Wait()
        {
            UCCard.CardSelected -= OnCardSelected;
            if (Game.o_deck.hand_cards.Count<7 && Game.p_deck.SqncEnd()) Game.p_deck.MoveToHand();
            
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
