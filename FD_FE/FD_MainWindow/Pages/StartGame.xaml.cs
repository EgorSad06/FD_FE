using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using FD_FE;

using FD_Tools.Connect;
using FD_Tools.Audio;

namespace FD_MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartGame.xaml
    /// </summary>
    
    public partial class StartGame : Page
    {
        //Для музыка
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();
        public StartGame()
        {
            InitializeComponent();
            
            Game.Cnct = new Connection();

            foreach (char frac in GameplayData.StartCards.Keys)
            {
                var new_but = new Button
                {
                    Style = (Style)Resources["S_frct_but"],
                    Tag = frac,
                    Background = new ImageBrush(Game.ToImg(@"CardTemplates\" + frac + "_template.png")) { Stretch = Stretch.UniformToFill }
                };
                new_but.Click += Mode_Click;
                SP_modes.Children.Add(new_but);
            }
        }

        // подключение
        private async Task Connect()
        {
            Game.Cnct.is_host = (bool)(RB_server.IsChecked);
            if (Game.Cnct.SetIP(TB_IP.Text))
            {
                B_connect.IsEnabled = false;
                B_connect.IsEnabled = !(B_start.IsEnabled = /*B_receive.IsEnabled = B_send.IsEnabled =*/ await Game.Cnct.Connect());
            }
        }
        private void Connect_Click(object sender, RoutedEventArgs e) => Connect();

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

        // режим игры
        private readonly bool[] slct_f = new bool[GameplayData.StartCards.Count];
        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            for (int i=0; i<GameplayData.StartCards.Count; i++)
            {
                if (GameplayData.StartCards.Keys.ElementAt(i) == ((Button)sender).Tag.ToString()[0]) {
                    slct_f[i] = ! slct_f[i];
                    ((Button)sender).BorderThickness = new Thickness( slct_f[i] ? 4 : 8 );
                }
            }
        }

        // игра
        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            short i = 0;
            //Отключение музыки
            AudioManager.MusicVolume = 0.0;
            for (int j=0; j< GameplayData.StartCards.Count; j++) if (slct_f[j]) i++;
            if (i != 0)
            {
                B_start.IsEnabled = false;
                if (Game.Cnct.is_host)
                {
                    i = ((await Game.Cnct.ReceiveDataS(1))[0] == i ? i : (short)0);
                    Game.Cnct.SendData(new byte[1] { (byte)i });
                }
                else
                {
                    Game.Cnct.SendData( new byte[1] { (byte)i });
                    i = ((await Game.Cnct.ReceiveDataS(1))[0] == i ? i : (short)0);
                }
                
                if (i != 0)
                {
                    if (Game.Cnct.is_host) Game.p_seed = Game.o_seed = BitConverter.ToInt32(await Game.Cnct.ReceiveData(4),0);
                    else Game.Cnct.SendData(BitConverter.GetBytes( Game.p_seed = Game.o_seed = Game.p_deck.SetSqnc() ));
                    for (int j = 0; j < GameplayData.StartCards.Count; j++) if (slct_f[j]) Game.slct_cards.deck_cards.AddRange(GameplayData.StartCards.ElementAt(j).Value);
                    Game.start_turn = Game.Cnct.is_host;
                    Game.SetMode(i);
                    NavigationService.Navigate(new Uri("Pages/CardSelection.xaml", UriKind.Relative));
                    NavigationService.RemoveBackEntry();
                }
                else B_start.IsEnabled = true;
            }
        }

        // выход
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Game.Cnct.Disconnect();
            NavigationService.Navigate(new Uri("Pages/MainMenu.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
            AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
        }

        private async void DB_Click(object sender, RoutedEventArgs e)
        {
            if ( !(bool) (RB_server.IsChecked = (string)((Button)sender).Tag == "t") )
                TB_IP.Text = "127.0.0.1";
            await Connect();
            Mode_Click(sender, e);
            StartButton_Click(sender, e);
        }
    }
}
