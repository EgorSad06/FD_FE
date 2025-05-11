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
using FD_FE;

namespace FD_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для UCCard.xaml
    /// </summary>
    public partial class UCCard : UserControl
    {
        public UCCard(BoardCard card, double x, double y)
        {
            this.BoardCard = card;
            InitializeComponent();
            string path = $"{GameplayData.sprites_path}CardTemplates/{this.BoardCard.fraction}_template.png";
            CardBackgound.ImageSource = (ImageSource)MainWindow.converter.ConvertFromString(path);
            path = $"{GameplayData.sprites_path}CardTemplates/{this.BoardCard.card_class}_frame.png";
            CardClassFrame.ImageSource = (ImageSource)MainWindow.converter.ConvertFromString(path);
            CardImage.Source = (ImageSource)MainWindow.converter.ConvertFromString($"{GameplayData.sprites_path}Cards/{this.BoardCard.image}");
            this.Margin = new Thickness(x, y, 0,0);
        }
        
        public static DependencyProperty BoardCardProperty;
        public BoardCard BoardCard
        {
            get { return (BoardCard)GetValue(BoardCardProperty); }
            set { SetValue(BoardCardProperty, value); }
        }
        static UCCard() { BoardCardProperty = DependencyProperty.Register("BoardCard", typeof(BoardCard), typeof(UCCard)); }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BoardAct.selected_card = BoardCard;
            BoardCard.AV += 1;
        }
    }
}
