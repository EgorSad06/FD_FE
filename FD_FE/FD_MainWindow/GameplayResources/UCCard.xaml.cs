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
            Width = cardVB.Width *= scale; Height = cardVB.Height *= scale;
            cardB.IsMouseDirectlyOverChanged += (object sender, DependencyPropertyChangedEventArgs e) => Panel.SetZIndex(UCcard, ((Button)sender).IsMouseDirectlyOver ? 1 : 0);
            //if (card.fr) добавить отдельное оформление
            if (BoardCard.GetFraction() == 'f')
            {
                // Пример: замени фон на "листики"
                CardImage.Source = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}Cards/{BoardCard.image}");
            }
          
            CardBackgound.ImageSource = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}CardTemplates/{BoardCard.GetFraction()}_template.png");
            CardClassFrame.ImageSource = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}CardTemplates/{BoardCard.card_class.id}_frame.png");
            CardImage.Source = (ImageSource)Game.converter.ConvertFromString($"{GameplayData.sprites_path}Cards/{BoardCard.image}");
            
        }

        private void CardB_IsMouseDirectlyOverChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public static DependencyProperty BoardCardProperty;
        public BoardCard BoardCard
        {
            get { return (BoardCard)GetValue(BoardCardProperty); }
            set { SetValue(BoardCardProperty, value); Update(); }
        }
        static UCCard() { BoardCardProperty = DependencyProperty.Register("BoardCard", typeof(BoardCard), typeof(UCCard)); }

        public void Update()
        {
            cardAV.Text = BoardCard.AV.ToString();
            cardHP.Text = BoardCard.HP.ToString();
        }

        public delegate void CardSelectedEventHandler(UCCard sender, BoardCard selected_card);
        static public event CardSelectedEventHandler CardSelected;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CardSelected?.Invoke(UCcard, UCcard.BoardCard);
        }
    }
}
