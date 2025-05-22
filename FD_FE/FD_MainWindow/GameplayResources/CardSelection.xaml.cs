using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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
        private static short[] p_slctd_card_i; // массив индексов выбранных карт для отправки
        private static short p_slctd_card_n;
        public CardSelection()
        {
            InitializeComponent();

            p_slctd_card_i = new short[Game.Mode.start_cards_count];
            p_slctd_card_n = 0;

            if (Game.game_start)
            { // начало игры (карты не выбираются)
                Game.slct_cards.SetSqnc();
                Game.game_start = false;
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++) {
                    Card temp = Game.slct_cards.GetCard();
                    card_slct_board.SetBoardCard(new BoardCard(temp, i));
                    Game.p_deck.deck_cards.Add(temp);
                    p_slctd_card_i[p_slctd_card_n] = (short)temp.id;
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
            p_slctd_card_i[p_slctd_card_n] = (short)card.id;
            sender.IsEnabled = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            But.Content = Game.p_deck.deck_cards.Count;
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Game.is_host)
            {
                byte[] slctd_card_i = await Game.ReceiveData(2 * Game.Mode.start_cards_count);
                for (int i=0; i<2*Game.Mode.start_cards_count; i++) Game.o_deck.deck_cards.Add(Game.StartCardByID( slctd_card_i[i]<<8 + slctd_card_i[++i]) );

                slctd_card_i = new byte[2 * Game.Mode.start_cards_count];
                for (int i = 0; i < Game.Mode.start_cards_count; i+=2)
                {
                    slctd_card_i[i] = (byte)(p_slctd_card_i[i] >> 8);
                    slctd_card_i[i+1] = (byte)(p_slctd_card_i[i]);
                }
                Game.SendData(slctd_card_i);
            }
            else
            {
                byte[] slctd_card_i = new byte[2 * Game.Mode.start_cards_count];
                for (int i = 0; i < Game.Mode.start_cards_count; i += 2)
                {
                    slctd_card_i[i] = (byte)(p_slctd_card_i[i] >> 8);
                    slctd_card_i[i + 1] = (byte)(p_slctd_card_i[i]);
                }
                Game.SendData(slctd_card_i);

                slctd_card_i = await Game.ReceiveData(2 * Game.Mode.start_cards_count);
                for (int i = 0; i < 2 * Game.Mode.start_cards_count; i++) Game.o_deck.deck_cards.Add(Game.StartCardByID(slctd_card_i[i] << 8 + slctd_card_i[++i]));
            }
            NavigationService.Navigate(new Uri("GameplayResources/Battle.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}
