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

namespace FD_MainWindow.GameplayPages
{
    /// <summary>
    /// Логика взаимодействия для StartGame.xaml
    /// </summary>
    public partial class StartGame : Page
    {
        public StartGame()
        {
            InitializeComponent();

        }
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            List<GameMode> gameModes = GameplayData.GameModes;
            Game.SetMode(0);
            NavigationService.Navigate(new Uri("GameplayResources/CardSelection.xaml", UriKind.Relative));
            // Если нужно закрыть текущую страницу:
            NavigationService.RemoveBackEntry();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
