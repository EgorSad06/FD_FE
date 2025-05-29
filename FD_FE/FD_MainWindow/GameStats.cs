using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD_MainWindow
{
    public static class GameStats
        //Файл модели статистики
    {
        public static TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero;
        public static int Wins { get; set; }
        public static int Losses { get; set; }
        public static int EnemiesKilled { get; set; }

        public static void AddPlayTime(TimeSpan sessionTime)
        {
            TotalPlayTime += sessionTime;
        }
    }
}
