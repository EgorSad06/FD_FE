﻿using System;
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

namespace FD_MainWindow.TutorialPages
{
    /// <summary>
    /// Логика взаимодействия для CardDetailFolk.xaml
    /// </summary>
    public partial class CardDetailFolk : Page
    {
        public CardDetailFolk()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на предыдущую страницу
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
                //звук
                AudioManager.PlayEffect("Assets/sound/listscroll.mp3");
            }
        }
    }
}
