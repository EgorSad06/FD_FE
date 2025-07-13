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
            BoardCard.CardChanged += Update;
            BoardCard.CardMoved += Update;
            BoardCard.CardStartedAct += (boardcard) => SetHighlight(100);
            BoardCard.CardEndedAct += (boardcard) => SetHighlight(0);

            Width = cardVB.Width *= scale; Height = cardVB.Height *= scale;

            cardAV.Style = (Style)Resources["S_cardav_" + BoardCard.GetFraction()];
            cardHP.Style = (Style)Resources["S_cardhp_" + BoardCard.GetFraction()];
            if (BoardCard.GetFraction() == 'f')
            {
                // Изменить фон и удалить имя
                cardBG.ImageSource = Game.ToImg("Cards\\" + BoardCard.image);

                cardG.Children.Remove(cardN);
                cardG.Children.Remove(cardCF);
                cardG.Children.Remove(cardI);
            }
            else
            {
                cardN.Style = (Style)Resources["S_cardn_" + BoardCard.GetFraction()];
                cardBG.ImageSource = Game.ToImg($"CardTemplates/{BoardCard.GetFraction()}_template.png");
                cardCF.Source = Game.ToImg($"CardTemplates/{BoardCard.card_class.id}_frame.png");
                cardI.Source = Game.ToImg($"Cards/{BoardCard.image}");
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
            Panel.SetZIndex(UCcard,
                opacity_percent == 0 ?
                -BoardCard.board_i : 1);
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
            CardSelected.Invoke(UCcard, UCcard.BoardCard);
        }
    }
    //public class CVConverter : IValueConverter
    //{
    //    public object Convert(object sender, Type Style, object parameter, CultureInfo info)
    //    {
    //        UCCard UCcard = (UCCard)sender;
    //        return (Style)(UCcard.Resources["S_cardav_" + (UCcard.BoardCard.GetFraction())]);
    //    }
    //    public object ConvertBack(object sender, Type Style, object parameter, CultureInfo info) => sender;
    //}
}
