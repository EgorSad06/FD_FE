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
        public static bool connected { get; set; }
        public static Socket socket { get; set; }
        public static IPAddress ip { get; private set; } = null;
        public static IPAddress SetIP() { ip = IPAddress.Any; return ip; }
        public static IPAddress SetIP(IPAddress new_ip) { ip = new_ip; return ip; }
        public static IPAddress SetIP(string new_ip) { ip = IPAddress.Parse(new_ip); return ip; }
        public static bool is_host { get; private set; }
        public static BackgroundWorker BGworker = new BackgroundWorker();
        public static TcpListener server = null;
        public static TcpClient client = null;

        public static void StartBGWork() { BGworker.RunWorkerAsync(); }
        public static void AddBGWork(DoWorkEventHandler func) { BGworker.DoWork += new DoWorkEventHandler( func); }
        public static void RemBGWork(DoWorkEventHandler func) { BGworker.DoWork -= func; }

        public static byte[] ReceiveData(int size)
        {
            byte[] data = new byte[size];
            socket.Receive(data);
            return data;
        }
        public static void SendData(byte[] data)
        {
            socket.Send(data);
        }
        
        public static async Task<bool>Connect(bool is_host_param = true)
        {
            is_host = is_host_param;
            if (is_host)
            {
                server = new TcpListener(ip, 4013);
                server.Start();
                socket = server.AcceptSocket();
                return true;
            }
            else
            {
                try
                {
                    client = new TcpClient(ip.ToString(), 4013);
                    socket = client.Client;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    client?.Close();
                    return false;
                }
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
