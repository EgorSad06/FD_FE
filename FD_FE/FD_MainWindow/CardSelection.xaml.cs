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

namespace FD_MainWindow
{
    /// <summary>
    /// Логика взаимодействия для CardSelection.xaml
    /// </summary>
    public partial class CardSelection : Page
    {
        public CardSelection()
        {
            InitializeComponent();
            SetCards();
        }
        public void SetCards()
        {
            CardSelectionGrid.Children.Add(new Border() { Width=40, Height=80, Background = new SolidColorBrush(Colors.DarkGray) });
        }
    }
}
