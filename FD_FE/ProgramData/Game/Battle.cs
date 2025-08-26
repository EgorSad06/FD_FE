using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace FD_FE.Stages
{
    public class Battle
    {
        public Grid Main_grid;
        public Grid Hand_grid;

        public Board Main_board = new Board(Game.Mode.board_width, Game.Mode.board_height, true);
        public Board Hand_board = new Board(1, 7);

        public bool p_turn = Game.start_turn;
        public BoardCard slct_card = null;
        public short[] card_act_sqnc = null;
        public short card_act_sqnc_i = 0;

        public short[] act_sqnc = null; // последовательность выбранных карт (индексы)
        public short act_sqnc_i = 0;

        public short[] slct_cards = null; // последовательность выставленных карт (индексы)
        public short slct_cards_i = 0;

        private short act_clr_n = 1; // число текущего раза cброса дейсвий
        public short turns { get; private set; } // удвоенное кол-во оставшихся ходов (завершённые ходы и игрока и противника уменьшают это число)

        public short p_score { get; private set; } = 0;
        public short o_score { get; private set; } = 0;

        public delegate void TurnEndEventHandler(char force);
        public event TurnEndEventHandler TurnEnd;
        public delegate void BattleResEventHandler(short p_score, short o_score, short turns);
        public event BattleResEventHandler ScoreChanged;
        public delegate void StateChangedEventHandler(short state);
        public event StateChangedEventHandler StateChanged;
        public event StateChangedEventHandler BattleEnded;
        // Состояния: 1 - ход 2 - назн действия карты 3 - ход противника -1 - победа -2 - ничья -3 - поражение

        public Battle(Grid main_grid, Grid hand_grid)
        {
            Main_grid = main_grid;
            Hand_grid = hand_grid;
        }
        // этапы игры
        public void Start()
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

            Main_board = new Board(Game.Mode.board_width, Game.Mode.board_height, true);
            Hand_board = new Board(1, 7);

            for (int i = 0; i < 3 && Game.p_deck.SqncEnd(); i++) // помещение в колоду 4 (3) стартовых карт
                Hand_board.SetBoardCard(Game.p_deck.NextToHand(), i);
            for (int i = 0; i < 3 && Game.o_deck.SqncEnd(); i++)
                Game.o_deck.NextToHand();

            turns = (short)(4 + Game.o_deck.deck_cards.Count + Game.p_deck.deck_cards.Count);

            Game.Draw(Hand_board, Hand_grid);
            Game.DrawPlayBoard(Main_board, Main_grid, (float)((Game.Mode.board_height == 3) ? 1.8 : 1.6));

            BoardCard.CardChanged += (card) => {
                if (card.HP <= 0)
                {
                    if (card.force != 'p') UpdateScore(1,0,0);
                    else UpdateScore(0,1,0);
                }
            };
            Main_board.BoardChanged += (sender, i) => Game.UpdatePlayBoard(sender, i, Main_grid, (float)((Game.Mode.board_height == 3) ? 1.8 : 1.6));
            Hand_board.BoardChanged += (sender, i) => Game.Update(sender, i, Hand_grid);
            UpdateScore(0, 0, 0);

            UCCard.CardSelected += OnCardSelected;
            TurnEnd += (force) =>
            {
                if (force == 'p') StartTurn();
                else Wait();
            };
            TurnEnd.Invoke(p_turn ? 'p' : 'o');
        }

        private void StartTurn()
        {
            StateChanged.Invoke(1);
            p_turn = true;
            slct_cards = new short[15];
            slct_cards_i = 0;
            act_sqnc = new short[Game.p_deck.deck_cards.Count * 5];
            act_sqnc_i = 0;
            UCSlot.SlotSelected += OnSlotSelected;
            if ((Game.p_deck.hand_cards.Count < 7 || Game.p_deck.hand_cards.Contains(null)) && Game.p_deck.SqncEnd())
                Hand_board.AddBoardCard(Game.p_deck.NextToHand());
        }

        public void Ready()
        {
            p_turn = false;
            slct_cards[slct_cards_i] = -1;
            Game.Cnct.SendData(slct_cards, 15);
            act_sqnc[act_sqnc_i] = -1;
            Game.Cnct.SendData(act_sqnc, Game.p_deck.deck_cards.Count * 5);

            EndTurn('o');
        }

        private async void Wait()
        {
            StateChanged.Invoke(3);
            UCSlot.SlotSelected -= OnSlotSelected;
            if ((Game.o_deck.hand_cards.Count < 7 || Game.o_deck.hand_cards.Contains(null)) && Game.o_deck.SqncEnd())
                Game.o_deck.NextToHand();

            if ((slct_cards = await Game.Cnct.ReceiveDataS(15)) == null) return;

            for (int i = 0; slct_cards?[i] != -1; i++)
                Main_board.SetBoardCard(Game.o_deck.RemFromHand(slct_cards[i]), Main_board.count - 1 - slct_cards[++i], 'o');

            if ((act_sqnc = await Game.Cnct.ReceiveDataS(Game.o_deck.deck_cards.Count * 5))==null) return;
            for (int i = 0; act_sqnc[i] != -1; i++)
                act_sqnc[i] = (short)(Main_board.count - act_sqnc[i] - 1);

            EndTurn('p');
        }

        public async void EndTurn(char next_force)
        {
            // действие
            for (int i = 0, j = 0; act_sqnc[i] != -1; i++)
            {
                short acting_card = act_sqnc[i];
                for (j = ++i; act_sqnc[j] != acting_card; j++) ;
                BoardCard[] targets = new BoardCard[j - i];
                for (int t = 0; i < j; t++, i++) targets[t] = Main_board.grid[act_sqnc[i]];
                await Main_board.grid[act_sqnc[j]]?.Act(targets, 500);
            }

            act_clr_n++;
            UpdateScore(0, 0, -1);

            // проверка конца поединка
            if (Math.Abs(p_score - o_score) == 5 || turns == 0)
            {
                UCSlot.SlotSelected -= OnSlotSelected;
                Game.p_deck.hand_cards.Clear();
                Game.o_deck.hand_cards.Clear();
                Game.battle++;
                short state;
                if (p_score > o_score)
                {
                    Game.o_global_score++;
                    Game.start_turn = false;
                    state = -1;
                }
                else if (p_score == o_score)
                {
                    Game.start_turn = !Game.start_turn;
                    state = -2;
                }
                else
                {
                    Game.p_global_score++;
                    Game.start_turn = true;
                    state = -3;
                }
                StateChanged.Invoke(state);
                BattleEnded.Invoke(state);
                return;
            };

            TurnEnd.Invoke(next_force);
        }

        // события хода
        private void UpdateScore(short p_s, short o_s, short t) =>
            ScoreChanged.Invoke(p_score += p_s, o_score += o_s, (short)(((turns += t) + 1) / 2));
        public void ClearAct()
        {
            act_clr_n++;
            Array.Clear(act_sqnc, 0, act_sqnc_i);
            act_sqnc_i = 0;
            card_act_sqnc = null;
            card_act_sqnc_i = 0;
            StateChanged.Invoke(1);
        }
        private void OnCardSelected(UCCard sender, BoardCard card)
        {
            slct_card = sender.BoardCard;
            if (!p_turn) return;
            if (sender.Parent == Main_grid) // если выбирается действие карты
            {
                if (card_act_sqnc == null)
                {
                    if (card.last_act_clr < act_clr_n && card.force == 'p')
                    {
                        card_act_sqnc = new short[card.select_n() + 2];
                        card_act_sqnc[0] = (short)card.board_i;
                        card_act_sqnc_i++;
                        StateChanged.Invoke(2);
                    }
                }
                else
                {
                    if (card == Main_board.grid[card_act_sqnc[0]]) // при досрочном завершении выбора действия
                        card_act_sqnc_i = card.select_n();
                    else
                    {
                        card_act_sqnc[card_act_sqnc_i] = (short)card.board_i;
                        card_act_sqnc_i++;
                    }
                    if (card_act_sqnc_i >= card.select_n()) // если выбор действия карты закончен
                    {
                        for (int i = 0; i <= Main_board.grid[card_act_sqnc[0]].select_n(); i++)
                            act_sqnc[act_sqnc_i++] = card_act_sqnc[i];
                        act_sqnc[act_sqnc_i++] = card_act_sqnc[0];
                        Main_board.grid[card_act_sqnc[0]].last_act_clr = act_clr_n;
                        card_act_sqnc = null;
                        card_act_sqnc_i = 0;
                        StateChanged.Invoke(1);
                    }
                }
            }
            else
            {
                card_act_sqnc = null;
                card_act_sqnc_i = 0;
                StateChanged.Invoke(1);
            }
        }

        private void OnSlotSelected(short i)
        {
            if (Hand_board.grid.Contains(slct_card))
            {
                slct_cards[slct_cards_i++] = (short)slct_card.board_i;
                int hand_card_id = slct_card.board_i;
                Main_board.SetBoardCard(Hand_board.RemBoardCard(slct_card.board_i), i);
                Game.p_deck.RemFromHand((short)hand_card_id);
                slct_cards[slct_cards_i++] = i;
            }
        }
    }
}
