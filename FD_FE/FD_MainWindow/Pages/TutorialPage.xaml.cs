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
using FD_Tools.Audio;

namespace FD_MainWindow.Pages
{
    public partial class TutorialPage : Page
    {
        public TutorialPage()
        {
            InitializeComponent();
        }

        private void Card1_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/TutorialPages/CardDetailFolk.xaml", UriKind.Relative));
            //звук
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }

        private void Card2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/TutorialPages/CardDetailTechno.xaml", UriKind.Relative));
            //звук
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }

        private void Card3_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CardDetail2());
            //звук
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }
        private void GoToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
            //звук
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }
        
    }
}