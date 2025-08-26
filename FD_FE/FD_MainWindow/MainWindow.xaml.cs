using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using FD_FE;
using FD_Tools.Audio;
using FD_Tools.Stats;


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
            //MainFrame.Content = new Pages.MainMenu();
            MainFrame.Content = new Pages.MainMenu();

            //громкость эффектов
            AudioManager.MusicVolume = 0.0; 
            AudioManager.InitMusic();

            // бинд F11 на фулл скрин, F12 на дебаг поединок
            KeyDown += (s, e) =>
            {
                if (e.Key == Key.F11)
                {
                    if (FD_window.WindowStyle == WindowStyle.None)
                    {
                        FD_window.WindowStyle = WindowStyle.SingleBorderWindow;
                        FD_window.ResizeMode = ResizeMode.CanResize;
                        FD_window.WindowState = WindowState.Normal;
                    }
                    else
                    {
                        FD_window.WindowStyle = WindowStyle.None;
                        FD_window.ResizeMode = ResizeMode.NoResize;
                        FD_window.WindowState = WindowState.Maximized;
                    }
                }
                else if (e.Key == Key.F12) MainFrame.Content = new Pages.DB_battle();
            };
        }

        private void MediaPlayer_Loop(object sender, EventArgs e)
        {
            _mediaPlayer.Position = TimeSpan.Zero;
            _mediaPlayer.Play();
        }

        private void FD_window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mediaPlayer.Stop();
            Game.Cnct.Disconnect();
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
