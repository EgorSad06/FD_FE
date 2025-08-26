using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Globalization;
using FD_FE;
using FD_Tools.Audio;

namespace FD_MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartGame.xaml
    /// </summary>
    
    public partial class StartGame : Page
    {
        private readonly FD_FE.Stages.StartGame _start_game = new FD_FE.Stages.StartGame();
        private readonly List<char> slct_f = new List<char>(GameplayData.StartCards.Count);
        //Для музыка
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        public StartGame()
        {
            InitializeComponent();

            B_start.IsEnabled = !(B_connect.IsEnabled = Game.Cnct.socket == null);
            foreach (char frct in GameplayData.StartCards.Keys)
            {
                var new_but = new Button
                {
                    Style = (Style)Resources["S_frct_but"],
                    Tag = frct,
                    Background = new ImageBrush(Game.ToImg(@"CardTemplates\" + frct + "_template.png")) { Stretch = Stretch.UniformToFill }
                };
                new_but.Click += Mode_Click;
                SP_modes.Children.Add(new_but);
            }
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            B_connect.IsEnabled = false;
            if (await _start_game.Connect((bool)RB_server.IsChecked, TB_IP.Text))
            {
                Game.Cnct.ConnectionInterrupted += (message) => MessageBox.Show("Ошибка передачи данных\n(" + message  + ')');
                Game.Cnct.SendRecieveInterrupted += (message) => MessageBox.Show("Подключение прервано\n(" + message  + ')');
                B_start.IsEnabled = true;
            }
            else B_connect.IsEnabled = true;
        }

        // чат - требует переработки (мб вообще не нужен)
        //private async void Receive_Click(object sender, RoutedEventArgs e)
        //{
        //    ((Button)sender).IsEnabled = false;
        //    Message.Text = Encoding.Unicode.GetString(await Game.ReceiveData(100));
        //    ((Button)sender).IsEnabled = true;
        //}
        //private void Send_Click(object sender, RoutedEventArgs e)
        //{
        //    Game.SendData(Encoding.Unicode.GetBytes(Message.Text + '\n'));
        //}

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            char f = ((Button)sender).Tag.ToString()[0];
            if (slct_f.Remove(f))
            {
                ((Button)sender).BorderThickness = new Thickness(8);
            }
            else
            {
                slct_f.Add(f);
                ((Button)sender).BorderThickness = new Thickness(4);
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            //Отключение музыки
            AudioManager.MusicVolume = 0.0;
            if (slct_f.Count != 0)
            {
                B_start.IsEnabled = false;
                if (await _start_game.TryStart(slct_f))
                {
                    NavigationService.Navigate(new Uri("Pages/CardSelection.xaml", UriKind.Relative));
                    NavigationService.RemoveBackEntry();
                }
                else B_start.IsEnabled = true;
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            _start_game.Disconnect();
            NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }

        private async void DB_Click(object sender, RoutedEventArgs e)
        {
            await _start_game.Connect( (bool)(RB_server.IsChecked = (string)((Button)sender).Tag == "t"), TB_IP.Text = "127.0.0.1");
            Mode_Click(sender, e);
            StartButton_Click(sender, e);
        }
    }
}
