using FD_FE;
using FD_MainWindow.GameplayPages;
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
using static System.Collections.Specialized.BitVector32;

namespace FD_MainWindow
{
    public partial class UCCard : UserControl
    {
        public UCCard() { InitializeComponent(); }

        public UCCard(BoardCard card, float scale=(float)1.8)
        {
            InitializeComponent();
            BoardCard = card;
            BoardCard.CardChanged += Update;
            BoardCard.CardMoved += Update;
            
            Width = cardVB.Width *= scale; Height = cardVB.Height *= scale;
            cardB.IsMouseDirectlyOverChanged += (object sender, DependencyPropertyChangedEventArgs e) => Panel.SetZIndex(UCcard, ((Button)sender).IsMouseDirectlyOver ? 1 : -BoardCard.board_i);
            if (BoardCard.GetFraction() == 'f')
            {
                // Изменить фон и удалить имя
                CardBackgound.ImageSource = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}Cards/{BoardCard.image}");
                CardGrid.Children.Remove(cardName);

                // AV справа сверху
                cardBorderAV.VerticalAlignment = VerticalAlignment.Top;
                cardBorderAV.HorizontalAlignment = HorizontalAlignment.Right;
                cardBorderAV.Margin = new Thickness(13, 2, 0, 3); // сверху и справа

                cardAV.VerticalAlignment = VerticalAlignment.Center;
                cardAV.Foreground = new SolidColorBrush(Colors.WhiteSmoke);

                // HP слева сверху
                cardHP.VerticalAlignment = VerticalAlignment.Top;
                cardHP.HorizontalAlignment = HorizontalAlignment.Left;
                cardHP.Margin = new Thickness(0, 2, 13, 3); // сверху и слева
                cardHP.Foreground = new SolidColorBrush(Colors.Black);
             

                /* 
                1 — нет отступа слева,

                2 — отступ сверху,

                3 — отступ справа,

                4 — нет отступа снизу. 
                */

                //размер и шрифт 
                cardHP.FontFamily = new FontFamily("Garamond");
                cardHP.FontSize = 16;
                cardHP.FontWeight = FontWeights.Bold;
                cardAV.FontFamily = new FontFamily("Garamond");
                cardAV.FontSize = 16;
                cardAV.FontWeight = FontWeights.Bold;
            }
            else
            {
                CardBackgound.ImageSource = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}CardTemplates/{BoardCard.GetFraction()}_template.png");
                CardClassFrame.ImageSource = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}CardTemplates/{BoardCard.card_class.id}_frame.png");
                CardImage.Source = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}Cards/{BoardCard.image}");
            }
        }

        public static DependencyProperty BoardCardProperty;
        public BoardCard BoardCard
        {
            get { return (BoardCard)GetValue(BoardCardProperty); }
            set { SetValue(BoardCardProperty, value); Update(); Update(BoardCard, BoardCard.board_i); }
        }
        static UCCard() { BoardCardProperty = DependencyProperty.Register("BoardCard", typeof(BoardCard), typeof(UCCard)); }

        public void Update(BoardCard sender=null)
        {
            cardAV.Text = BoardCard.AV.ToString();
            cardHP.Text = BoardCard.HP.ToString();
        }
        public void Update(BoardCard sender, int i)
        {
            Panel.SetZIndex(UCcard, -BoardCard.board_i);
        }

        public delegate void CardSelectedEventHandler(UCCard sender, BoardCard selected_card);
        static public event CardSelectedEventHandler CardSelected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CardSelected?.Invoke(UCcard, UCcard.BoardCard);
        }
    }
}
