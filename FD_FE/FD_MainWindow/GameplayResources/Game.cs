using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using FD_FE;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;

namespace FD_MainWindow
{
    public static class Game
    {
        // данные игры
        public static GameMode Mode { get; private set; }
        public static void SetMode(short mode) { Mode = GameplayData.GameModes[mode]; }

        public static Deck slct_cards = new Deck(GameplayData.StartCards); // колода карт для выбора
        public static Deck p_deck = new Deck();

        public static bool game_started = true;
        
        // отрисовка
        static public ImageSourceConverter converter = new ImageSourceConverter();
        static public void Draw(Board board, Grid grid, short a, short b) 
        {
            double dx = grid.Width / a, dy = grid.Height / b;
            for (int j = 0, i = 0; j < b && i + j < board.grid.Count; j++)
                for (i = 0; i < a && i + j < board.grid.Count; i++)
                    Draw(board.grid[i + j], grid, dx * i, dy * j);
        }
        static public UCCard Draw(BoardCard card, Grid grid, double x, double y)
        {
            if (card == null) return null;
            else
            {
                UCCard uc_card = new UCCard(card, x, y);
                uc_card.HorizontalAlignment = HorizontalAlignment.Left; uc_card.VerticalAlignment = VerticalAlignment.Top;
                grid.Children.Add(uc_card);
                return uc_card;
            }
        }

        // подключение
        public static Socket socket { get; set; }
        public static string ip { get; private set; } = null;
        public static bool is_host { get; set; }
        public static BackgroundWorker BGworker = new BackgroundWorker();
        public static TcpListener server = null;
        public static TcpClient client = null;

         public static void StartBackgroundWork()
        {

        }
        public static byte[] ReceiveData(int size)
        {
            byte[] data = new byte[size];
            socket.Receive(data);
            return data;
        }
        public static void SendData(byte[] data)
        {
            socket.Send(data);
            BGworker.RunWorkerAsync();
        }
        
        public static string Connect(bool is_host = true, string ip = null)
        {
            Game.is_host = is_host;
            if (is_host)
            {
                IPAddress new_ip = IPAddress.Any;
                server = new TcpListener(new_ip, 4013);
                server.Start();
                socket = server.AcceptSocket();
                return new_ip.ToString();
            }
            else
            {
                try
                {
                    client = new TcpClient(ip, 4013);
                    socket = client.Client;
                    BGworker.RunWorkerAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    client.Close();
                }
                return ip;
            }
        }
        public static void Disconnect()
        {
            BGworker.WorkerSupportsCancellation = true;
            BGworker.CancelAsync();
            server?.Stop();
        }
    }
}
