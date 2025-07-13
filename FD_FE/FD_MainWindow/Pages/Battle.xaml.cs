using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Логика взаимодействия для Battle.xaml
    /// </summary>
    public partial class Battle : Page
    {
        public Battle()
        {
            InitializeComponent();
            Start();
        }

        public Board Main_board = new Board(Game.Mode.board_width, Game.Mode.board_height);
        public Board Hand_board = new Board(1, 7);

        public bool p_turn = Game.Cnct.is_host;
        public BoardCard slct_card = null;
        public short[] card_act_sqnc = null;
        public short card_act_sqnc_i = 0;

        public short[] act_sqnc = null; // последовательность выбранных карт (индексы)
        public short act_sqnc_i = 0;

        public short[] slct_cards = null; // последовательность выставленных карт (индексы)
        public short slct_cards_i = 0;

        private short act_clr_n = -1; // число текущего раза cброса дейсвий
        public static short turns { get; private set; } // удвоенное кол-во оставшихся ходов (завершённые ходы и игрока и противника уменьшают это число)

        public short p_score { get; private set; } = 0;
        public  short o_score { get; private set; } = 0;
        public void Set_p_score(short new_score) { p_score = new_score; UpdateScore(); }
        public void Set_o_score(short new_score) { p_score = new_score; UpdateScore(); }

        private delegate void TurnEndEventHandler();
        private event TurnEndEventHandler TurnEnd;

        // этапы игры
        private void Start()
        {
            if (Game.Cnct.is_host) // перемешивание колоды
            {
                Game.p_seed *= 4013;
                Game.o_seed /= 4013;
            }
            else
            {
                Game.p_seed /= 4013;
                Game.o_seed *= 4013;
            }
            Game.p_deck.SetSqnc(Game.p_seed);
            Game.o_deck.SetSqnc(Game.o_seed);

            Main_board = new Board(Game.Mode.board_width, Game.Mode.board_height);
            Hand_board = new Board(1, 7);

            for (int i = 0; i < 3 && Game.p_deck.SqncEnd(); i++) // помещение в колоду 4 (3) стартовых карт
                Hand_board.SetBoardCard(Game.p_deck.MoveToHand(), i);
            for (int i = 0; i < 3 && Game.o_deck.SqncEnd(); i++)
                Game.o_deck.MoveToHand();

            turns = (short)( 4 + Game.o_deck.deck_cards.Count + Game.p_deck.deck_cards.Count );

            Game.Draw(Hand_board, HandBoardGrid);
            Game.DrawPlayBoard(Main_board, MainBoardGrid, (float) ((Game.Mode.board_height == 3) ? 1.8 : 1.6));

            BoardCard.GlobalCardChanged += (BoardCard card) => {
                if (card.HP <= 0)
                {
                    if (card.force != 'p')
                    {
                        p_score++;
                        App.CurrentStats.EnemiesKilled++;
                    }
                    else o_score++;
                    UpdateScore();
                }
            };
            Main_board.BoardChanged += (Board sender, int i) => { Game.UpdatePlayBoard(sender, i, MainBoardGrid, (float)((Game.Mode.board_height == 3) ? 1.8 : 1.6)); };
            Hand_board.BoardChanged += (Board sender, int i) => { Game.Update(sender, i, HandBoardGrid); };
            UpdateScore();

            UCCard.CardSelected += OnCardSelected;
            B_ready.IsEnabled = Game.start_turn;

            if (p_turn) TurnEnd += StartTurn;
            else TurnEnd += Wait;
            TurnEnd.Invoke();
        }

        private void StartTurn()
        {
            State.Text = "Ход";
            p_turn = true;
            B_ready.IsEnabled = true;
            slct_cards = new short[15];
            slct_cards_i = 0;
            act_sqnc = new short[Game.p_deck.deck_cards.Count * 5] ;
            act_sqnc_i = 0;
            UCSlot.SlotSelected += OnSlotSelected;
            if ( (Game.p_deck.hand_cards.Count<7 || Game.p_deck.hand_cards.Contains(null)) && Game.p_deck.SqncEnd() )
                Hand_board.AddBoardCard(Game.p_deck.MoveToHand());
        }

        private void Ready_Click(object sender, RoutedEventArgs e)
        {
            B_ready.IsEnabled = false;
            p_turn = false;
            slct_cards[slct_cards_i] = -1;
            Game.Cnct.SendData(slct_cards, 15);
            act_sqnc[act_sqnc_i] = -1;
            Game.Cnct.SendData(act_sqnc, Game.p_deck.deck_cards.Count * 5);

            TurnEnd -= StartTurn;
            TurnEnd += Wait;
            NextTurn();
        }

        private async void Wait()
        {
            State.Text = "Ход\nсоперника";
            UCSlot.SlotSelected -= OnSlotSelected;
            if ( (Game.o_deck.hand_cards.Count < 7 || Game.o_deck.hand_cards.Contains(null)) && Game.o_deck.SqncEnd() )
                Game.o_deck.MoveToHand();

            slct_cards = await Game.Cnct.ReceiveDataS(15);

            for (int i = 0; slct_cards?[i]!=-1; i++)
                Main_board.SetBoardCard(Game.o_deck.MoveFromHand(slct_cards[i]), Main_board.count-1-slct_cards[++i], 'o');

            act_sqnc = await Game.Cnct.ReceiveDataS(Game.o_deck.deck_cards.Count * 5);
            for (int i = 0; act_sqnc[i]!=-1; i++)
                act_sqnc[i] = (short)(Main_board.count - act_sqnc[i] - 1);

            TurnEnd -= Wait;
            TurnEnd += StartTurn;
            NextTurn();
        }

        public async void NextTurn() {
            // действие
            for (int i = 0, j = 0; act_sqnc[i] != -1; i++)
            {
                short acting_card = act_sqnc[i];
                for (j = ++i; act_sqnc[j] != acting_card; j++) ;
                BoardCard[] targets = new BoardCard[j - i];
                for (int t = 0; i < j; t++, i++) targets[t] = Main_board.grid[act_sqnc[i]];
                await Main_board.grid[act_sqnc[j]]?.Act(targets, 500);
            }
            // сократить число ходов
            turns--;
            UpdateScore();
            // проверка конца игры
            if (Math.Abs(p_score - o_score) == 5 || turns == 0) { End_Battle(); return; };

            act_clr_n++;

            TurnEnd.Invoke();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            act_clr_n++;
            act_sqnc = new short[Game.p_deck.deck_cards.Count * 5];
            act_sqnc_i = 0;
        }

        private void End_Battle()
        {
            UCSlot.SlotSelected -= OnSlotSelected;

            Game.p_deck.hand_cards.Clear();
            Game.o_deck.hand_cards.Clear();

            if (p_score > o_score)
            {
                Game.o_global_score++;
                Game.start_turn = false;
                MessageBox.Show("Вы победили!");
            }
            else if (p_score == o_score)
            {
                Game.start_turn = !Game.start_turn;
                MessageBox.Show("Ничья");
            }
            else
            {
                Game.p_global_score++;
                Game.start_turn = true;
                MessageBox.Show("Вы проиграли");
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

                if (Game.p_global_score> Game.o_global_score)
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
        
        // события хода
        private void UpdateScore()
        {
            TB_o_score.Text = o_score.ToString();
            TB_p_score.Text = p_score.ToString();
            TB_turns.Text = ((turns + 1) / 2).ToString();
        }

        private void OnCardSelected(UCCard sender, BoardCard card)
        {
            slct_card = sender.BoardCard;
            if (!p_turn) return;
            if (sender.Parent == MainBoardGrid) // если выбирается действие карты
            {
                if (card_act_sqnc == null)
                {
                    if (card.last_act_clr != act_clr_n && card.force == 'p') {
                        card_act_sqnc = new short[card.select_n(card) + 2];
                        card_act_sqnc[0] = (short)card.board_i;
                        card_act_sqnc_i++;
                        State.Text = "Назначение\nдействия";
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
                        Main_board.grid[card_act_sqnc[0]].last_act_clr = act_clr_n;
                        card_act_sqnc = null;
                        card_act_sqnc_i = 0;
                        State.Text = "Ход";
                    }
                }
            }
            else
            {
                card_act_sqnc = null;
                card_act_sqnc_i = 0;
                State.Text = "Ход";
            }
        }

        private void OnSlotSelected(short i)
        {
            if (Hand_board.grid.Contains(slct_card))
            {
                slct_cards[slct_cards_i++] = (short)slct_card.board_i;
                int hand_card_id = slct_card.board_i;
                Main_board.SetBoardCard(Hand_board.RemBoardCard(slct_card.board_i), i);
                Game.p_deck.MoveFromHand((short)hand_card_id);
                slct_cards[slct_cards_i++] = i;
            }
        }
    }
}
