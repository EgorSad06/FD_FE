using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FD_MainWindow
{
    public class GameStats
        //Файл модели статистики
    {

        public TimeSpan TotalPlayTime { get; set; } = TimeSpan.Zero;
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int EnemiesKilled { get; set; }

        public void AddPlayTime(TimeSpan sessionTime)
        {
            TotalPlayTime += sessionTime;
        }

    }
}
