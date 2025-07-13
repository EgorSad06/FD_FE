using System;
using System.IO;
using Newtonsoft.Json;

namespace FD_Tools.Stats
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
    public class StatsManager
    {
        private static readonly string FilePath = "statistics.json";

        public static GameStats LoadStats()
        {
            if (!File.Exists(FilePath))
                return new GameStats();

            try
            {
                string json = File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<GameStats>(json) ?? new GameStats();
            }
            catch (JsonException)
            {
                // Если файл повреждён — возвращаем новый объект
                return new GameStats();
            }
        }

        public static void SaveStats(GameStats stats)
        {
            string json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }
    }
}