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
using System.Threading;

namespace FD_MainWindow
{
    public static class Game
    {
        // данные игры
        public static GameMode Mode { get; private set; }
        public static void SetMode(short mode) { Mode = GameplayData.GameModes[mode]; }

        public static bool game_start = true;

        public static byte[] act_sqnc = null;
        public static Deck slct_cards = new Deck();
        public static Deck p_deck = new Deck();
        public static Deck o_deck = new Deck();

        public static Card StartCardByID(int card_id) {
            foreach (List<Card> f_list in GameplayData.StartCards.Values) foreach (Card card in f_list) if (card.id == card_id) return card;
            return null;
        }

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
                UCCard uc_card = new UCCard(card);
                uc_card.HorizontalAlignment = HorizontalAlignment.Left; uc_card.VerticalAlignment = VerticalAlignment.Top;
                uc_card.Margin = new Thickness(x, y, 0, 0);
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
        public static async Task<byte[]> ReceiveData(int n, Socket skt = null) => await Task<byte[]>.Run(() =>
        {
            if (skt == null) skt = socket;
            try
            {
                byte[] data = new byte[n];
                socket.Receive(data);
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        });
        public static async Task<short[]> ReceiveDataS(int n, Socket skt = null) => await Task<short[]>.Run(() =>
        {
            if (skt == null) skt = socket;
            try
            {
                byte[] data = new byte[2 * n];
                short[] res = new short[n];
                socket.Receive(data);
                for (int i = 0; i < n; i++)
                {
                    res[i] = (short)(( (data[i * 2+1]) << 8) + data[i * 2]);
                }
                return res;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        });
        //public static async Task<object[]> ReceiveData(int n, int size) => await Task<object[]>.Run(()=>
        //{
        //    try
        //    {
        //        byte[] data = new byte[size*n];
        //        object[] res = new object[n];
        //        socket.Receive(data);
        //        long e = 0;
        //        for (int i=0; i<n; i++)
        //        {
        //            for (int j=0;  j<size; j++)
        //            {
        //                e = e << 8 + data[i*size + j];
        //            }
        //            res[i] = e;
        //        }
        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return null;
        //    }
        //});
        //public static void SendData(object[] data, int n, int size=2)
        //{
        //    byte[] res = new byte[size * n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        long e = (long)data[i];
        //        for (int j = 0; j < size; j++)
        //        {
        //            res[i*size + j] = (byte)e;
        //            e = e >> 8;
        //        }
        //    }
        //    try { socket.Send(res); }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //}
        public static void SendData(byte[] data, Socket skt = null)
        {
            if (skt == null) skt = socket;
            try { socket.Send(data); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        public static void SendData(short[] data, int n, Socket skt = null)
        {
            if (skt == null) skt = socket;
            byte[] res = new byte[2 * n];
            for (int i = 0; i < n; i++)
            {
                res[i * 2] = (byte)data[i];
                res[i * 2 + 1] = (byte)(data[i]>>8);
                short e = data[i];
            }
            try { socket.Send(res); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public static async Task<bool> Connect()
        {
            if (is_host)
            {
                server = new TcpListener(ip, 4013);
                server.Start();
                return await Task<bool>.Run(()=> {
                    try
                    {
                        socket = server.AcceptSocket();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        server?.Stop();
                        return false;
                    }
                });
            }
            else
            {
                return await Task<bool>.Run(()=>
                {
                    try {
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
                });
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
