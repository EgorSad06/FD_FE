using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FD_FE.Stages
{
    public class CardSelection
    {
        public Board Card_slct_board = new Board(Game.Mode.start_cards_count, 1); // поле карт для выбора
        public Grid Card_slct_grid;
        private short[] slct_cards_i; // массив индексов выбранных карт для отправки
        private short slct_cards_n;

        public CardSelection(Grid card_slct_grid, float card_size)
        {
            Card_slct_grid = card_slct_grid;

            slct_cards_i = new short[Game.Mode.start_cards_count];
            slct_cards_n = 0;

            if (Game.battle == 0)
            { // начало игры (карты не выбираются)
                Game.slct_cards.SetSqnc(new Random().Next() + ((Game.Cnct.is_host) ? 4013 : -4013));
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++)
                {
                    Card_slct_board.SetBoardCard(Game.slct_cards.NextToHand(), i);
                    Game.p_deck.deck_cards.Add(Game.slct_cards.hand_cards[i]);
                    slct_cards_i[slct_cards_n++] = (short)Game.slct_cards.hand_cards[i].id;
                }
                Game.Draw(Card_slct_board, Card_slct_grid, card_size);
                foreach (UCCard uc_card in Card_slct_grid.Children) uc_card.IsEnabled = false;
            }
            else
            { // игра (карты выбираются)
                UCCard.CardSelected += AddCardToDeck;
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++)
                    Card_slct_board.SetBoardCard(Game.slct_cards.NextToHand(), i);
                Game.Draw(Card_slct_board, Card_slct_grid, card_size);
            }
        }
        public void AddCardToDeck(UCCard sender, BoardCard card)
        {
            Game.p_deck.deck_cards.Add(Game.slct_cards.hand_cards[card.board_i]);
            slct_cards_i[slct_cards_n++] = (short)card.id;
            sender.IsEnabled = false;
        }
        public async Task Start()
        {
            Game.Cnct.SendData(slct_cards_i, Game.Mode.start_cards_count);
            short[] shorts = await Game.Cnct.ReceiveDataS(Game.Mode.start_cards_count);
            for (int i = 0; i < Game.Mode.start_cards_count; i++) if (shorts[i] != 0) Game.o_deck.deck_cards.Add(Game.StartCardByID(shorts[i]));

            Game.slct_cards.hand_cards.Clear();
            UCCard.CardSelected -= AddCardToDeck;
        }
    }
}
