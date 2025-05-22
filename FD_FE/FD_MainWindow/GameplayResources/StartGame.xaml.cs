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
using FD_FE;

namespace FD_MainWindow.GameplayPages
{
    /// <summary>
    /// Логика взаимодействия для StartGame.xaml
    /// </summary>
    public partial class StartGame : Page
    {
        public StartGame()
        {
            InitializeComponent();
        }
        private bool[] slct_f = new bool[GameplayData.StartCards.Count];
        private async void Receive_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            Message.Text = Encoding.Unicode.GetString( await Game.ReceiveData(100));
            ((Button)sender).IsEnabled = true;
        }
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Game.SendData(Encoding.Unicode.GetBytes(Message.Text));
        }

        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            Game.is_host = (bool)(RB_server.IsChecked);
            Game.SetIP(TB_IP.Text);
            B_connect.IsEnabled = false;
            B_connect.IsEnabled = !(B_start.IsEnabled = B_receive.IsEnabled = B_send.IsEnabled = await Game.Connect());
        }

        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).Tag = (((Button)sender).Tag.ToString()[0]=='1') ? "0"+((Button)sender).Tag.ToString()[1] : "1" + ((Button)sender).Tag.ToString()[1];
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            short i = -1;
            foreach (Button button in SP_modes.Children) {
                if (button.Tag.ToString()[0] == '1')
                {
                    Game.slct_cards.deck_cards.AddRange(GameplayData.StartCards[ button.Tag.ToString()[1] ]);
                    i++;
                }
            }
            if (Game.is_host)
            {
                i = ((await Game.ReceiveData(1))[0]==i) ? i : (short)-1;
                Game.SendData(new byte[1] { (byte)(i << 8) });
            }
            else
            {
                Game.SendData( new byte[1] { (byte)(i<<8) } );
                i = ((await Game.ReceiveData(1))[0] == i) ? i : (short)-1;
            }
            if (i != -1)
            {
                Game.SetMode(i);
                NavigationService.Navigate(new Uri("GameplayResources/CardSelection.xaml", UriKind.Relative));
                // Если нужно закрыть текущую страницу:
                NavigationService.RemoveBackEntry();
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Game.Disconnect();
            NavigationService.Navigate(new Uri("MainMenu.xaml", UriKind.Relative));
            // Если нужно закрыть текущую страницу:
            NavigationService.RemoveBackEntry();
        }
    }
}
