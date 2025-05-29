using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            this.Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                // Обновление интерфейса
                GameStats stats = App.CurrentStats;

                TotalTimeText.Text = stats.TotalPlayTime.ToString(@"hh\:mm\:ss");
                GamesPlayedText.Text = (stats.Wins + stats.Losses).ToString();
                WinsText.Text = stats.Wins.ToString();
                LossesText.Text = stats.Losses.ToString();
                EnemiesKilledText.Text = stats.EnemiesKilled.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке статистики: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();

                // Воспроизведение звука при возврате
                MediaPlayer mediaPlayer = new MediaPlayer();
                mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
                mediaPlayer.Play();
            }
        }
    }
}