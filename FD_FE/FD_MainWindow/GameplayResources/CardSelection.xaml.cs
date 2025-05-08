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
    /// Логика взаимодействия для CardSelection.xaml
    /// </summary>
    public partial class CardSelection : Page
    {
        public CardSelection()
        {
            InitializeComponent();
            Deck slct_cards = new Deck(CardsData.StartCards); // колода для выбора из неё карт
            slct_cards.SetSqnc();

            Board card_slct_board = new Board();
            for (int i = 0; i < 2; i++) // отображение карт для выбора на поле
            {
                card_slct_board.SetBoardCard(new BoardCard(slct_cards.GetCard()));
            }
            MainWindow.Draw(CardSelectionGrid, card_slct_board);


        }
        //public void SetCards()
        //{
        //    CardSelectionGrid.Children.Add(new Border() { Width=40, Height=80, Background = new SolidColorBrush(Colors.DarkGray) });
        //}
    }
}
