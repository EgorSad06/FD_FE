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
            B_connect.IsEnabled = !(B_receive.IsEnabled = B_send.IsEnabled = await Game.Connect());
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Game.SetMode(0);
            NavigationService.Navigate(new Uri("GameplayResources/CardSelection.xaml", UriKind.Relative));
            // Если нужно закрыть текущую страницу:
            NavigationService.RemoveBackEntry();
        }
    }
}
