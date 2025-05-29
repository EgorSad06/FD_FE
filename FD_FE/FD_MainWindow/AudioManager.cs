using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FD_MainWindow
{
    internal class AudioManager
    {
        public static MediaPlayer MusicPlayer { get; private set; } = new MediaPlayer();

        public static double MusicVolume
        {
            get => MusicPlayer.Volume;
            set => MusicPlayer.Volume = value;
        }

        private static double _effectsVolume = 1.0;
        public static double EffectsVolume
        {
            get => _effectsVolume;
            set => _effectsVolume = value;
        }

        public static void PlayEffect(string path)
        {
            MediaPlayer player = new MediaPlayer();
            player.Open(new Uri(path, UriKind.RelativeOrAbsolute));
            player.Volume = EffectsVolume;
            player.Play();

            // Освободить ресурс после окончания
            player.MediaEnded += (s, e) => player.Close();
        }

        public static void InitMusic()
        {
            MusicPlayer.Open(new Uri("Assets/sound/BGsound.mp3", UriKind.RelativeOrAbsolute));
            MusicPlayer.MediaEnded += (s, e) =>
            {
                MusicPlayer.Position = TimeSpan.Zero;
                MusicPlayer.Play();
            };
            MusicPlayer.Volume = MusicVolume;
            MusicPlayer.Play();
        }

        public static void StopMusic()
        {
            MusicPlayer.Stop();
        }
    }
}
