using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace FD_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private DispatcherTimer _playTimer;
        private DateTime _lastTick;

        public static GameStats CurrentStats { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            // Для отладки привязок
            PresentationTraceSources.DataBindingSource.Switch.Level = SourceLevels.Warning;

            base.OnStartup(e);

            // Загружаем статистику
            CurrentStats = StatsManager.LoadStats();

            // Настраиваем таймер для подсчёта времени
            _lastTick = DateTime.Now;
            _playTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _playTimer.Tick += PlayTimer_Tick;
            _playTimer.Start();
        }

        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var delta = now - _lastTick;
            _lastTick = now;

            // Увеличиваем общее время
            CurrentStats.TotalPlayTime += delta;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // Сохраняем статистику при выходе
            StatsManager.SaveStats(CurrentStats);
            base.OnExit(e);
        }
    }
}