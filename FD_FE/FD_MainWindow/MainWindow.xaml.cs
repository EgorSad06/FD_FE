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

        static ImageSourceConverter converter = new ImageSourceConverter();

        static public void Draw(Grid grid, Board board)
        {
            for (int i=0; i<board.grid.Count; i++) if (board.grid[i]!=null) Draw(grid, board.grid[i]);
        }
        static public void Draw(Grid grid, BoardCard card)
        {
            var new_card_header = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 92, Height = 29,
                Margin = new Thickness(0, 6, 0, 0)
            };
            var slctr = new StyleSelector();
            //new_card_header.Children.Add(
            //new TextBlock() { Style = slctr.SelectStyle() }
            //);
            var new_card = new StackPanel()
            {
                Orientation = Orientation.Vertical,
                Width = 100,
                Height = 160,
                Background = new ImageBrush((ImageSource)converter.ConvertFrom("../../../ProgramData/Assets/Sprites/CardBasics/general_template.png")) { Stretch=Stretch.UniformToFill },
            };
            grid.Children.Add(new StackPanel() { });
        }
    }
}
