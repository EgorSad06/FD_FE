using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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

namespace FD_MainWindow.Pages
{
    /// <summary>
    /// Логика взаимодействия для CardSelection.xaml
    /// </summary>
    public partial class CardSelection : Page
    {
        private readonly FD_FE.Stages.CardSelection _card_selection;
        public CardSelection()
        {
            InitializeComponent();
            _card_selection = new FD_FE.Stages.CardSelection(CardSelectionGrid, (float)2.5);
        }

        private async void Start_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
            await _card_selection.Start();
            NavigationService.Navigate(new Uri("Pages/Battle.xaml", UriKind.Relative));
            NavigationService.RemoveBackEntry();
        }
    }
}
