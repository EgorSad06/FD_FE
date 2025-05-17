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

namespace FD_MainWindow
{
    public static class Game
    {
        // режим игры
        public static GameMode Mode { get; private set; }
        public static void SetMode(short mode) { Mode = GameplayData.GameModes[mode]; }

        public static Deck slct_cards = new Deck(GameplayData.StartCards); // колода карт для выбора
        public static Deck p_deck = new Deck();

        public static bool game_started = true;
        
        public static Socket socket { get; set; }
        public static BackgroundWorker DataReceiver = new BackgroundWorker();
        public static TcpListener server = null;
        public static TcpClient client = null;

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
        public static byte[] RecieveData(int size)
        {
            byte[] data = new byte[size];
            socket.Receive(data);
            return data;
        }
    }
}
