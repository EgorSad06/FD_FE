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
        public static Board Main_board = new Board();
        public Battle()
        {
            InitializeComponent();

            foreach (Card card in Game.o_deck.deck_cards) Main_board.SetBoardCard(card);
            Game.Draw(Main_board, MainBoardGrid, Game.Mode.board_length, Game.Mode.board_width);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
