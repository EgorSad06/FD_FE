using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace FD_MainWindow
{
    internal class StatsManager
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