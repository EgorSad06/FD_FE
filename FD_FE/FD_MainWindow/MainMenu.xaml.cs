using FD_FE;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;



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
        //старт
        private void PlayButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // Анимация исчезновения текущей страницы (0.5 секунд)
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(1)
            };

            fadeOut.Completed += (s, _) =>
            {
                NavigationService.RemoveBackEntry();
                // Переход на новую страницу через URI
                NavigationService.Navigate(new Uri("GameplayResources/StartGame.xaml", UriKind.Relative));
            };

            // Запуск анимации
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);

            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
          //  mediaPlayer.Close();
        }
        //обучение
        private void EducationButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // Анимация исчезновения текущей страницы (0.5 секунд)
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            fadeOut.Completed += (s, _) =>
            {
                NavigationService.RemoveBackEntry();
                // Переход на новую страницу через URI
                NavigationService.Navigate(new Uri("TutorialPage.xaml", UriKind.Relative));
            };

            // Запуск анимации
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }
        //статистика
        private void StatisticsButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // Анимация исчезновения текущей страницы (0.5 секунд)
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            fadeOut.Completed += (s, _) =>
            {
                NavigationService.RemoveBackEntry();
                // Переход на новую страницу через URI
                NavigationService.Navigate(new Uri("Statistic.xaml", UriKind.Relative));
            };

            // Запуск анимации
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
        }
        //Настройки
        private void SettingsButtonSelection_Click(object sender, RoutedEventArgs e)
        {
            // Анимация исчезновения текущей страницы (0.5 секунд)
            DoubleAnimation fadeOut = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.5)
            };

            fadeOut.Completed += (s, _) =>
            {
                NavigationService.RemoveBackEntry();
                // Переход на новую страницу через URI
                NavigationService.Navigate(new Uri("Setting.xaml", UriKind.Relative));
            };

            // Запуск анимации
            this.BeginAnimation(UIElement.OpacityProperty, fadeOut);
            //звук
            MediaPlayer mediaPlayer = new MediaPlayer();
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            mediaPlayer.Open(new Uri("Assets/sound/listscroll.mp3", UriKind.RelativeOrAbsolute));
            mediaPlayer.Play();
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
        //Загрузка страниц для анимации
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Storyboard fadeIn = (Storyboard)FindResource("FadeInAnimation");
                fadeIn.Begin(this); // Указываем целевой объект (саму страницу)
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка анимации: {ex.Message}");
            }
        }
    }
}
