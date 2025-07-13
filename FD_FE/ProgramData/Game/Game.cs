using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using FD_Tools.Connect;

namespace FD_FE
{
    public static class Game
    {
        // данные игры
        public static GameMode Mode { get; private set; }
        public static void SetMode(short mode) { Mode = GameplayData.GameModes[mode - 1]; }
        public static Connection Cnct;
        public static short battle = 0;

        public static Deck slct_cards = new Deck();
        public static Deck p_deck = new Deck();
        public static Deck o_deck = new Deck();

        public static bool start_turn = false;
        public static int p_seed;
        public static int o_seed;
        public static short p_global_score = 0;
        public static short o_global_score = 0;

        public static Card StartCardByID(int card_id)
        {
            foreach (List<Card> f_list in GameplayData.StartCards.Values) foreach (Card card in f_list) if (card.id == card_id) return card;
            return null;
        }

        // отрисовка
        private static ImageSourceConverter converter = new ImageSourceConverter();
        public static ImageSource ToImg(string path) => (ImageSource)converter.ConvertFrom(@"..\..\..\ProgramData\Assets\Sprites\" + path);

        public static void Draw(Board board, Grid grid, float scale = (float)1.8) // для поля
        {
            double dx = grid.Width / board.width, dy = grid.Height / board.height;
            for (int i = 0; i < board.count; i++)
                if (board.grid[i] != null)
                    Draw(board.grid[i], grid, dx * (i % board.width + 0.5), dy * (i / board.width + 0.5), scale);
        }
        public static void DrawPlayBoard(Board board, Grid grid, float scale = (float)1.8) // для поля с доп слотами
        {
            double dx = grid.Width / board.width, dy = grid.Height / board.height;
            for (int i = 0; i < board.count; i++)
            {
                if (board.grid[i] != null) Draw(board.grid[i], grid, dx * (i % board.width + 0.5), dy * (i / board.width + 0.5), scale);
                else Draw((short)i, grid, dx * (i % board.width + 0.5), dy * (i / board.width + 0.5), scale);
            }
        }
        public static UCCard Draw(BoardCard card, Grid grid, double x, double y, float scale = (float)1.8) // для карточки
        {
            UCCard uc_card = new UCCard(card, scale)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            uc_card.Margin = new Thickness(x - uc_card.Width * 0.5, y - uc_card.Height * 0.5, -200, -200);
            grid.Children.Add(uc_card);
            return uc_card;
        }
        public static UCSlot Draw(short i, Grid grid, double x, double y, float scale = (float)1.8) // для слота
        {
            UCSlot uc_slot = new UCSlot(i, scale)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            uc_slot.Margin = new Thickness(x - uc_slot.Width * 0.5, y - uc_slot.Height * 0.5, -200, -200);
            grid.Children.Add(uc_slot);
            return uc_slot;
        }

        public static void Update(Board board, int i, Grid grid, float scale = (float)1.8) // для поля (может иметь пустые указатели)
        {
            UCCard uc_card = null;
            foreach (UCCard e in grid.Children) if (e.BoardCard.board_i == i) { uc_card = e; break; }
            if (uc_card == null)
            {
                if (board.grid[i] != null)
                    Draw(board.grid[i], grid,
                        (grid.Width / board.width * (i % board.width + 0.5)),
                        grid.Height / board.height * (i / board.width + 0.5),
                        scale
                    );
            }
            else
            {
                if (board.grid[i] != null)
                {
                    uc_card.Margin = new Thickness(
                        (grid.Width / board.width * (i % board.width + 0.5)) - uc_card.Width * 0.5,
                        (grid.Height / board.height * (i / board.width + 0.5)) - uc_card.Height * 0.5,
                        -200, -200);
                }
                else grid.Children.Remove(uc_card);
            }
        }
        public static void UpdatePlayBoard(Board board, int i, Grid grid, float scale) // для поля (пустые указатели - слоты)
        {
            foreach (UIElement e in grid.Children)
            {
                try
                {
                    if (((UCCard)e).BoardCard.board_i == i)
                    {
                        if (board.grid[i] == null)
                        {
                            grid.Children.Remove(e);
                            Draw((short)i, grid,
                                grid.Width / board.width * (i % board.width + 0.5),
                                grid.Height / board.height * (i / board.width + 0.5),
                                scale
                            );
                        }
                        else if (board.grid[i] != ((UCCard)e).BoardCard)
                        {
                            grid.Children.Remove((UCCard)e);
                            Draw(board.grid[i], grid,
                                grid.Width / board.width * (i % board.width + 0.5),
                                grid.Height / board.height * (i / board.width + 0.5),
                                scale
                            );
                        }
                        break;
                    }
                }
                catch
                {
                    if (((UCSlot)e).board_i == i)
                    {
                        if (board.grid[i] != null)
                        {
                            grid.Children.Remove((UCSlot)e);
                            Draw(board.grid[i], grid,
                                grid.Width / board.width * (i % board.width + 0.5),
                                grid.Height / board.height * (i / board.width + 0.5),
                                scale
                            );
                        }
                        break;
                    }
                }
            }
        }
    }
}
