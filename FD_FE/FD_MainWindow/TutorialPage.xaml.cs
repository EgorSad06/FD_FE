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

namespace FD_MainWindow
{
    public partial class TutorialPage : Page
    {

        public TutorialPage()
        {
            InitializeComponent();

        }

        private void Card1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("TutorialPages/CardDetailFolk.xaml", UriKind.Relative));
        }

        private void Card2_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CardDetail2());
        }

        private void Card3_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CardDetail3());
        }

        private void Card4_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CardDetail4());
        }

        private void GoToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
        
    }
}