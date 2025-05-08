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
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UCCard : UserControl
    {
        public UCCard()
        {
            InitializeComponent();
        }
        
        public static DependencyProperty BoardCardProperty;
        public BoardCard BoardCard
        {
            get { return (BoardCard)GetValue(BoardCardProperty); }
            set { SetValue(BoardCardProperty, value); }
        }
        static UCCard() { BoardCardProperty = DependencyProperty.Register("BoardCard", typeof(BoardCard), typeof(UCCard)); }

        //public static readonly RoutedEvent CardClickedEvent = EventManager.RegisterRoutedEvent("CardClicked", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<ClickMode>), typeof(UCCard));
        //public event RoutedPropertyChangedEventHandler<ClickMode> CardClicked
        //{
        //    add { AddHandler(CardClickedEvent, value); }
        //    remove { RemoveHandler(CardClickedEvent, value); }
        //}
    }
}
