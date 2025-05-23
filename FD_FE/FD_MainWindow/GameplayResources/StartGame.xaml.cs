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
            for (int i=0; i<GameplayData.StartCards.Count; i++)
            {
                if (GameplayData.StartCards.Keys.ElementAt(i) == ((Button)sender).Tag.ToString()[0]) {
                    slct_f[i] = ! slct_f[i];
                    ((Button)sender).BorderThickness = new Thickness( slct_f[i] ? 2 : 4 );
                }
            }
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            short i = 0;
            for (int j=0; j< GameplayData.StartCards.Count; j++) i += (short) (slct_f[j] ? 1:0);
            if (i != 0)
            {
                B_start.IsEnabled = false;
                Message.Text = i.ToString();
                if (Game.is_host)
                {
                    i = (await Game.ReceiveData(1))[0]==i ? i : (short)0;
                    Game.SendData(new byte[1] { (byte)i });
                }
                else
                {
                    Game.SendData( new byte[1] { (byte)i } );
                    i = (await Game.ReceiveData(1))[0]==i ? i : (short)0;
                }
                
                if (i != 0)
                {
                    foreach (Button button in SP_modes.Children) if (slct_f[i])
                        {
                            Game.slct_cards.deck_cards.AddRange(GameplayData.StartCards[button.Tag.ToString()[0]]);
                            i++;
                        }
                    Game.SetMode(i);
                    NavigationService.Navigate(new Uri("GameplayResources/CardSelection.xaml", UriKind.Relative));
                    // Если нужно закрыть текущую страницу:
                    NavigationService.RemoveBackEntry();
                }
                else B_start.IsEnabled = true;
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
