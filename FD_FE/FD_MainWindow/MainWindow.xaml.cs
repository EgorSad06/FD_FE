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
        private MediaPlayer _mediaPlayer = new MediaPlayer();

        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Content = new MainMenu();

            // Настройка пути и воспроизведение
            _mediaPlayer.Open(new Uri("Assets/sound/BGsound.mp3", UriKind.RelativeOrAbsolute)); ;
            _mediaPlayer.MediaEnded += MediaPlayer_Loop; // Цикличное воспроизведение
            _mediaPlayer.Volume = 0; // можно настроить громкость
            _mediaPlayer.Play();
        }

        private void MediaPlayer_Loop(object sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.Zero;
            _mediaPlayer.Play();
        }

        private void FD_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mediaPlayer.Stop();
            Game.Disconnect();
        }

        //сбор статистики
        private DateTime _sessionStart;
        private GameStats _currentStats;

        public void StartGame()
        {
            _sessionStart = DateTime.Now;
            _currentStats = StatsManager.LoadStats();
        }

        public void EndGame(int wins, int losses, int kills)
        {
            TimeSpan playTime = DateTime.Now - _sessionStart;
            _currentStats.AddPlayTime(playTime);
            _currentStats.Wins += wins;
            _currentStats.Losses += losses;
            _currentStats.EnemiesKilled += kills;

            StatsManager.SaveStats(_currentStats);
        }
    }
}
