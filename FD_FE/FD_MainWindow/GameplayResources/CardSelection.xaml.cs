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

            Deck slct_cards = new Deck(GameplayData.StartCards); // колода карт для выбора
            slct_cards.SetSqnc();
            Board card_slct_board = new Board(); // поле карт для выбора

            for (int i=0; i<Game.gamemode.start_cards_count && slct_cards.SqncEnd(); i++) card_slct_board.SetBoardCard(new BoardCard(slct_cards.GetCard()));

            MainWindow.Draw(card_slct_board, CardSelectionGrid, Game.gamemode.start_cards_count, 1); // отображение карт для выбора на поле


        }

    }
}
