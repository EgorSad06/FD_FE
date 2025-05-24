using FD_FE;
using FD_MainWindow.GameplayPages;
using System;
using System.Collections.Generic;
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

        public UCCard(BoardCard card)
        {
            InitializeComponent();
            BoardCard = card;
            BoardCard.CardChanged += Update;

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
