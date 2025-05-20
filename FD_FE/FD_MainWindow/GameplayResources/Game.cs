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
        public static bool is_host = true;
        private static IPAddress ip = IPAddress.Any;
        public static IPAddress GetIP() => ip;
        public static bool SetIP(IPAddress IP) { ip = (is_host) ? IPAddress.Any : IP; return true; }
        public static bool SetIP(string IP) {
            if (is_host) { ip = IPAddress.Any; return true; }
            else return IPAddress.TryParse(IP, out ip);
        }


        public static TcpListener server = null;
        public static TcpClient client = null;
        public static Socket socket = null;

        public static async Task<byte[]> ReceiveData(int size)
        {
            byte[] data = new byte[size];
            try {
                await Task.Run(() => socket.Receive(data));
                return data;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return null; 
            }
        }
        public static void SendData(byte[] data)
        {
            try { socket.Send(data); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        
        public static async Task<bool> Connect()
        {
            if (is_host)
            {
                server = new TcpListener(ip, 4013);
                server.Start();
                await Task.Run(()=>socket = server.AcceptSocket());
                return true;
            }
            else
            {
                try
                {
                    await Task.Run(()=>client = new TcpClient(ip.ToString(), 4013));
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
            ip = IPAddress.Any;
            socket?.Close();
            server?.Stop();
            client?.Close();
        }
    }
}
