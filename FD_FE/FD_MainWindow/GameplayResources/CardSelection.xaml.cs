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
        public static Board card_slct_board = new Board(Game.Mode.start_cards_count, 1); // поле карт для выбора
        private static short[] p_slctd_card_i; // массив индексов выбранных карт для отправки
        private static short p_slctd_card_n;
        public CardSelection()
        {
            InitializeComponent();

            p_slctd_card_i = new short[Game.Mode.start_cards_count];
            p_slctd_card_n = 0;

            if (Game.battle==0)
            { // начало игры (карты не выбираются)
                Random random = new Random();
                Game.slct_cards.SetSqnc(random.Next()+((Game.is_host)?4013:-4013));
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++) {
                    Card temp = Game.slct_cards.GetCard();
                    card_slct_board.SetBoardCard(temp, i);
                    Game.p_deck.deck_cards.Add(temp);
                    p_slctd_card_i[p_slctd_card_n++] = (short)temp.id;
                }
                Game.Draw(card_slct_board, CardSelectionGrid);
                foreach (UCCard uc_card in CardSelectionGrid.Children) uc_card.IsEnabled = false;
            }
            else
            { // игра (карты выбираются)
                UCCard.CardSelected += AddCardToDeck;
                for (int i = 0; i < Game.Mode.start_cards_count && Game.slct_cards.SqncEnd(); i++) card_slct_board.SetBoardCard(Game.slct_cards.GetCard(), i);
                Game.Draw(card_slct_board, CardSelectionGrid);
            }
        }
        private void AddCardToDeck(UCCard sender, BoardCard card)
        {
            Game.p_deck.deck_cards.Add(card.source);
            p_slctd_card_i[p_slctd_card_n++] = (short)card.id;
            sender.IsEnabled = false;
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            if (Game.is_host)
            {
                ((Button)sender).IsEnabled = false;
                short[] shorts = await Game.ReceiveDataS(Game.Mode.start_cards_count);
                for (int i=0; i< Game.Mode.start_cards_count; i++) if (shorts[i]!=0) Game.o_deck.deck_cards.Add(Game.StartCardByID(shorts[i]));
                Game.SendData(p_slctd_card_i, Game.Mode.start_cards_count);
            }
            else
            {
                ((Button)sender).IsEnabled = false;
                Game.SendData(p_slctd_card_i, Game.Mode.start_cards_count);
                short[] shorts = await Game.ReceiveDataS(Game.Mode.start_cards_count);
                for (int i = 0; i < Game.Mode.start_cards_count; i++) if (shorts[i] != 0) Game.o_deck.deck_cards.Add(Game.StartCardByID(shorts[i]));
            }
            NavigationService.Navigate(new Uri("GameplayResources/Battle.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}
