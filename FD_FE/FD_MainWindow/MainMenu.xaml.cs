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
    /// Логика взаимодействия для MainMenu.xaml
    /// файл настройки кнопок и функций Меню.
    /// </summary>
    public partial class MainMenu : Page
    {
        public MainMenu()
        {
            InitializeComponent();
        }
        // кнопка играть, переход к созданию комнаты
        private void PlayButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("GameplayPages/StartGame.xaml", UriKind.Relative));
            // Если нужно закрыть текущую страницу:
            NavigationService.RemoveBackEntry();
        }
        //кнопка обучение, переход к обучению
        private void EducationButtonSelection_Click(object sender, RoutedEventArgs e)
        {

        }
        // Кнопка статистики, переход к просмотру статистики 
        private void StatisticsButtonSelection_Click(object sender, RoutedEventArgs e)
        {

        }
        //Кнопка Настройки, переход к настройкам игры
        private void SettingsButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // Переход на страницу настроек
            NavigationService.Navigate(new Uri("Setting.xaml", UriKind.Relative));

            // Если нужно закрыть текущую страницу:
            NavigationService.RemoveBackEntry();
        }
        //выход из игры
        private void ExitButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // предупреждение о выходе
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти?", "Подтверждение",
                                           MessageBoxButton.YesNo, MessageBoxImage.Question);

            // проверка ответа пользователя
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

        }





    }
}
