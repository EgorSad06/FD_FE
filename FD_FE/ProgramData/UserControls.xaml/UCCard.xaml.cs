using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace FD_FE
{
    public partial class UCCard : UserControl
    {
        public UCCard() => InitializeComponent();

        public UCCard(BoardCard card, float scale = (float)1.8)
        {
            InitializeComponent();
            BoardCard = card;
            card.LocalCardChanged += Update;
            card.CardMoved += Update;
            card.CardStartedAct += (boardcard) => SetHighlight(90);
            card.CardEndedAct += (boardcard) => SetHighlight(0);
            cardB.IsMouseDirectlyOverChanged += (s, e) => SetHighlight((bool)e.NewValue ? 55 : 0);

            Width = cardVB.Width *= scale; Height = cardVB.Height *= scale;

            string style = "_" + Game.GetFraction(BoardCard.id);
            if (card.force != 'p')
            {
                style += "_r";
                cardG.Background.Transform = new RotateTransform(180, 50, 80);
                cardCF.RenderTransform = new RotateTransform(180, 45.75, 74.75);
                cardI.Margin = new Thickness(0,0,0,10);
            }
            cardAV.Style = (Style)Resources["S_av" + style];
            cardHP.Style = (Style)Resources["S_hp" + style];
            if (Game.GetFraction(card.id) == 'f')
            {
                cardBG.ImageSource = Game.ToImg("Cards\\" + card.image);

                cardG.Children.Remove(cardN);
                cardG.Children.Remove(cardCF);
                cardG.Children.Remove(cardI);
            }
            else
            {
                cardN.Style = (Style)Resources["S_n" + style];
                cardBG.ImageSource = Game.ToImg($"CardTemplates/{Game.GetFraction(card.id)}_template.png");
                cardCF.Source = Game.ToImg($"CardTemplates/{card.card_class.id}_frame.png");
                cardI.Source = Game.ToImg($"Cards/{card.image}");
            }
        }

        public static DependencyProperty BoardCardProperty;
        public BoardCard BoardCard
        {
            get { return (BoardCard)GetValue(BoardCardProperty); }
            set { SetValue(BoardCardProperty, value); Update(); Update(BoardCard, BoardCard.board_i); }
        }
        static UCCard() { BoardCardProperty = DependencyProperty.Register("BoardCard", typeof(BoardCard), typeof(UCCard)); }

        public void SetHighlight(int opacity_percent)
        {
            UCcard.Background.Opacity = opacity_percent / 100.0;
            if (opacity_percent == 0)
            {
                Panel.SetZIndex(UCcard, -BoardCard.board_i);
                cardB.BorderThickness = new Thickness(1.5);
            }
            else
            {
                Panel.SetZIndex(UCcard, 1);
                cardB.BorderThickness = new Thickness(2.5);
            }
        }
        public void Update(BoardCard sender = null)
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
    //public class CVConverter : IValueConverter
    //{
    //    public object Convert(object sender, Type Style, object parameter, CultureInfo info)
    //    {
    //        UCCard UCcard = (UCCard)sender;
    //        return (Style)(UCcard.Resources["S_av_" + (UCcard.BoardCard.GetFraction())]);
    //    }
    //    public object ConvertBack(object sender, Type Style, object parameter, CultureInfo info) => sender;
    //}
}
