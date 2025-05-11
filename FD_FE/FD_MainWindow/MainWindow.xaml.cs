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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new MainMenu();
        }

        static public ImageSourceConverter converter = new ImageSourceConverter();
        static public void Draw(Board board, Grid grid, short a, short b) // отрисовка поля карт
        {
            double dx = grid.Width / a, dy = grid.Height / b;
            for (int j=0, i=0; j<b && i+j < board.grid.Count; j++)
                for (i=0; i<a && i+j < board.grid.Count; i++)
                    Draw(board.grid[i + j], grid, dx * i, dy * j);
        }
        static public UCCard Draw(BoardCard card, Grid grid, double x, double y) // отрисовка карты
        {
            if (card == null)return null;
            else {
                UCCard uc_card = new UCCard(card, x, y);
                uc_card.HorizontalAlignment = HorizontalAlignment.Left; uc_card.VerticalAlignment = VerticalAlignment.Top;
                grid.Children.Add(uc_card);
                return uc_card;
            }
        }

    }
}
