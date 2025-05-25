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
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }

        private void Card2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("TutorialPages/CardDetailTechno.xaml", UriKind.Relative));
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }

        private void Card3_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new CardDetail2());
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }
        private void GoToMenu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }
        
    }
}