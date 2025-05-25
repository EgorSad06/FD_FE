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
    /// <summary>
    /// Логика взаимодействия для Statistic.xaml
    /// </summary>
    public partial class Statistic : Page
    {

        public Statistic()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на предыдущую страницу
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                //звук
                MediaPlayer mediaPlayer = new MediaPlayer();
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
                mediaPlayer.Play();
            }
        }
    }
}
