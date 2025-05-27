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
using System.Windows.Shapes;

namespace FD_MainWindow
{
    public static class Game
    {
        // данные игры
        public static GameMode Mode { get; private set; }
        public static void SetMode(short mode) { Mode = GameplayData.GameModes[mode-1]; }

        public static short battle = 0;

        public static Deck slct_cards = new Deck();
        public static Deck p_deck = new Deck();
        public static Deck o_deck = new Deck();

        public static bool start_turn = false;
        public static int p_seed;
        public static int o_seed;
        public static short p_global_score = 0;
        public static short o_global_score = 0;

        public static Card StartCardByID(int card_id) {
            foreach (List<Card> f_list in GameplayData.StartCards.Values) foreach (Card card in f_list) if (card.id == card_id) return card;
            return null;
        }

        // отрисовка
        static public ImageSourceConverter converter = new ImageSourceConverter();
        static public void Draw(Board board, Grid grid) // для поля
        {
            double dx = grid.Width / board.width, dy = grid.Height / board.height;
            for (int i = 0; i < board.count; i++)
                if (board.grid[i] != null)
                    Draw(board.grid[i], grid, dx * (i % board.width + 0.5), dy * (i / board.width + 0.5));
        }
        static public void Draw(Board board, Grid grid, float scale) // для поля с доп слотами
        {
            double dx = grid.Width / board.width, dy = grid.Height / board.height;
            for (int i=0; i<board.count; i++)
            {
                if (board.grid[i] != null) Draw(board.grid[i], grid, dx * (i %board.width +0.5), dy * (i/board.width +0.5), scale);
                else Draw((short)i, grid, dx * (i % board.width + 0.5), dy * (i / board.width + 0.5), scale);
            }
        }
        static public UCCard Draw(BoardCard card, Grid grid, double x, double y, float scale =(float)1.8) // для карточки
        {
            UCCard uc_card = new UCCard(card, scale);
            uc_card.HorizontalAlignment = HorizontalAlignment.Left; uc_card.VerticalAlignment = VerticalAlignment.Top;
            x -= uc_card.Width * 0.5; y -= uc_card.Height * 0.5;
            uc_card.Margin = new Thickness(x, y, 0, 0);
            grid.Children.Add(uc_card);
            return uc_card;
        }
        static public UCSlot Draw(short i, Grid grid, double x, double y, float scale = (float)1.8) // для слота
        {
            UCSlot uc_slot = new UCSlot(i, scale);
            uc_slot.HorizontalAlignment = HorizontalAlignment.Left; uc_slot.VerticalAlignment = VerticalAlignment.Top;
            uc_slot.Margin = new Thickness(x-uc_slot.Width*0.5, y-uc_slot.Height*0.5, 0, 0);
            grid.Children.Add(uc_slot);
            return uc_slot;
        }
        static public void Update(Board board, Grid grid)
        {
            for (int i=0; i<grid.Children.Count; i++)
            {
                UCCard e = (UCCard)grid.Children[i];
                if (e.BoardCard != board.grid[e.BoardCard.board_i])
                {
                    grid.Children.RemoveAt(i);
                    grid.Children.Add(Draw(
                        board.grid[e.BoardCard.board_i], grid,
                        (float)(grid.Width / board.width * (i % board.width + 0.5)),
                        (float)(grid.Height / board.height * (i % board.height + 0.5))
                    ));
                }
            }
        }
        static public void Update(Board board, Grid grid, float scale)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                try
                {
                    UCCard e = (UCCard)grid.Children[i];
                    if (e.BoardCard != board.grid[e.BoardCard.board_i])
                    {
                        grid.Children.RemoveAt(i);
                        Draw(board.grid[e.BoardCard.board_i], grid,
                            (float)(grid.Width / board.width * (i % board.width + 0.5)),
                            (float)(grid.Height / board.height * (i % board.height + 0.5))
                        );
                    }
                }
                catch
                {
                    UCSlot e = (UCSlot)grid.Children[i];
                    if (board.grid[e.board_i] != null)
                    {
                        grid.Children.RemoveAt(i);
                        Draw(e.board_i, grid,
                            (float)(grid.Width / board.width * (i % board.width + 0.5)),
                            (float)(grid.Height / board.height * (i % board.height + 0.5))
                        );
                    }
                }
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
