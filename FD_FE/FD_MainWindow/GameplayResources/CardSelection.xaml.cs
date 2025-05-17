using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для CardSelection.xaml
    /// </summary>
    public partial class CardSelection : Page
    {
        public static Board card_slct_board = new Board(); // поле карт для выбора
        public CardSelection()
        {
            InitializeComponent();

            if (Game.game_started)
            { // начало игры (карты не выбираются)
                Game.slct_cards.SetSqnc();
                Game.game_started = false;
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++) {
                    Card temp = Game.slct_cards.GetCard();
                    card_slct_board.SetBoardCard(new BoardCard(temp));
                    Game.p_deck.deck_cards.Add(temp);
                }
                Game.Draw(card_slct_board, CardSelectionGrid, Game.Mode.start_cards_count, 1);
                foreach (UCCard uc_card in CardSelectionGrid.Children) uc_card.IsEnabled = false;
            }
            else
            { // игра (карты выбираются)
                UCCard.CardSelected += AddCardToDeck;
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++) card_slct_board.SetBoardCard(new BoardCard(Game.slct_cards.GetCard()));
                Game.Draw(card_slct_board, CardSelectionGrid, Game.Mode.start_cards_count, 1);
            }
        }
        private void AddCardToDeck(UCCard sender, BoardCard card)
        {
            Game.p_deck.deck_cards.Add(card.source);
            sender.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            But.Content = Game.p_deck.deck_cards.Count;
        }
    }
}
