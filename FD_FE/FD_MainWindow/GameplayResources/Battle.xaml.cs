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

        public static bool p_turn;
        public static BoardCard slct_card = null;
        public static short[] card_act_sqnc = null;
        public static short card_act_sqnc_i = 0;

        public static short[] act_sqnc = null; // последовательность выбранных карт
        public static short act_sqnc_i = 0;

        public static short[] slct_cards = null; // последовательность выставленных карт
        public static short slct_cards_i = 0;
        public static short turns { get; private set; } // удвоенное кол-во ходов (завершённые ходы и игрока и противника уменьшают это число)
        public static void Dec_turns() { turns--; ScoreChanged?.Invoke(); }

        private delegate void ScoreChangedEventHandler();
        private static ScoreChangedEventHandler ScoreChanged;
        public static short p_score { get; private set; } = 0;
        public static short o_score { get; private set; } = 0;
        public static void Set_p_score(short new_score) { p_score = new_score; ScoreChanged?.Invoke(); }
        public static void Set_o_score(short new_score) { p_score = new_score; ScoreChanged?.Invoke(); }
        private bool Check_End() => Math.Abs(p_score - o_score) == 5 || turns == 0;


        // начало игры
        private void Start()
        {
            if (Game.is_host) // перемешивание колоды
            {
                Game.p_seed *= 4013;
                Game.o_seed /= 4013;
                Game.p_deck.SetSqnc(Game.p_seed);
                Game.o_deck.SetSqnc(Game.o_seed);
            }
            else
            {
                Game.p_seed /= 4013;
                Game.o_seed *= 4013;
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

            ScoreChanged += () => {
                TB_o_score.Text = o_score.ToString();
                TB_p_score.Text = p_score.ToString();
                TB_turns.Text = ((turns + 1)/2).ToString();
            };
            TB_turns.Text = ( (( turns=(short)(4+(Game.o_deck.deck_cards.Count+Game.p_deck.deck_cards.Count)/2*2) )+1)/2 ).ToString();

            Game.Draw(Hand_board, HandBoardGrid);
            Game.Draw(Main_board, MainBoardGrid, (float) ((Game.Mode.board_height == 3) ? 1.8 : 1.6));

            Main_board.BoardChanged += (Board sender, int i) => { Game.Update(sender, i, MainBoardGrid, (float)((Game.Mode.board_height == 3) ? 1.8 : 1.6)); };
            Hand_board.BoardChanged += (Board sender, int i) => { Game.Update(sender, i, HandBoardGrid); };

            UCCard.CardSelected += OnCardSelected;
            B_ready.IsEnabled = Game.start_turn;
            if (p_turn = Game.start_turn) Turn();
            else Wait();
        }

// события хода
        private void OnCardSelected(UCCard sender, BoardCard card)
        {
            slct_card = sender.BoardCard;
            if (p_turn)
            {
                if (sender.Parent == MainBoardGrid) // если выбирается действие карты
                {
                    if (card_act_sqnc == null)
                    {
                        if (card.card_act && card.force == 'p') {
                            card_act_sqnc = new short[card.select_n(card) + 1];
                            card_act_sqnc[0] = (short)card.board_i;
                            card_act_sqnc_i++;
                        }
                    }
                    else
                    {
                        if (card == Main_board.grid[card_act_sqnc[0]]) // при досрочном завершении выбора действия
                            card_act_sqnc_i = card.select_n(card);
                        else
                        {
                            card_act_sqnc[card_act_sqnc_i] = (short)card.board_i;
                            card_act_sqnc_i++;
                        }
                        if (card_act_sqnc_i >= card.select_n(card)) // если выбор действия карты закончен
                        {
                            for (int i = 0; i <= Main_board.grid[card_act_sqnc[0]].select_n(Main_board.grid[card_act_sqnc[0]]); i++)
                                act_sqnc[act_sqnc_i++] = card_act_sqnc[i];
                            act_sqnc[act_sqnc_i++] = card_act_sqnc[0];
                            card.card_act = false;
                        }
                    }
                }
                else
                {
                    card_act_sqnc = null;
                    card_act_sqnc_i = 0;
                }
            }
        }
        private void OnSlotSelected(short i)
        {
            if (Hand_board.grid.Contains(slct_card))
            {
                slct_cards[slct_cards_i++] = (short)slct_card.board_i;
                Main_board.SetBoardCard(Hand_board.RemBoardCard(slct_card.board_i), i);
                slct_cards[slct_cards_i++] = i;
            }
        }

        private void Act()
        {
            for (int i=0; act_sqnc[i]!=-1; i++)
            {

            }
        }

// управление этапами игры
        private void Turn()
        {
            B_ready.IsEnabled = true;
            if (Check_End()) End_Battle();
            slct_cards = new short[14];
            slct_cards_i = 0;
            act_sqnc = new short[Game.p_deck.deck_cards.Count * 5] ;
            act_sqnc_i = 0;
            UCSlot.SlotSelected += OnSlotSelected;
            if (Game.p_deck.hand_cards.Count<7 && Game.p_deck.SqncEnd()) Hand_board.AddBoardCard(Game.p_deck.MoveToHand());
            foreach (UIElement uc_card in MainBoardGrid.Children) { try { ((UCCard)uc_card).BoardCard.card_act = true; } catch { } }
            

        }
        private async void Wait()
        {
            if (Check_End()) End_Battle();
            UCSlot.SlotSelected -= OnSlotSelected;
            if (Game.o_deck.hand_cards.Count < 7 && Game.o_deck.SqncEnd()) Game.o_deck.MoveToHand();

            slct_cards = await Game.ReceiveDataS(14);

            if (Game.o_deck.SqncEnd()) Game.o_deck.MoveToHand();
            for (int i = 0; slct_cards[i]!=-1; i++)
            {
                Main_board.SetBoardCard(Game.o_deck.hand_cards[slct_cards[i]], Main_board.count-1-slct_cards[++i], 'o');
            }

            act_sqnc = await Game.ReceiveDataS(Game.o_deck.deck_cards.Count * 5);
            for (int i = 0; act_sqnc[i]!=-1; i++)
            {
                act_sqnc[i] = (short)(Main_board.count - act_sqnc[i] - 1);
            }

            // ACT

            Dec_turns();

            Turn();
        }

        private void Ready_Click(object sender, RoutedEventArgs e)
        {
            B_ready.IsEnabled = false;
            slct_cards[slct_cards_i] = -1;
            Game.SendData(slct_cards, 14);
            act_sqnc[act_sqnc_i] = -1;
            Game.SendData(act_sqnc, Game.p_deck.deck_cards.Count * 5);

            // ACT

            Dec_turns();
            Wait();
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
                p_score = 0;
                o_score = 0;
                NavigationService.Navigate(new Uri("GameplayResources/GameStart.xaml", UriKind.Relative));
                NavigationService.RemoveBackEntry();
            }
        }
    }
}
