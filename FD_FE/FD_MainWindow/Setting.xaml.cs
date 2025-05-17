using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class Setting : Page
    {
        // Коллекция доступных разрешений
        //public ObservableCollection<Resolution> Resolutions { get; } = new ObservableCollection<Resolution>()
        //{
        //    new Resolution(1920, 1080),
        //    new Resolution(1280, 720),
        //    new Resolution(1600, 900),
        //    new Resolution(100,100)
        //};

        // Текущие настройки
        private int _brightness = 80;
        private int _masterVolume = 100;
        private int _effectsVolume = 100;

        public Setting()
        {
            InitializeComponent();

            // Инициализация элементов управления
            //ResolutionComboBox.ItemsSource = Resolutions;
            ResolutionComboBox.SelectedIndex = 0;

            //В будующем можно добавить звуки эффектов, музыку 

            WindowedRadioButton.IsChecked = false;
            //BrightnessSlider.Value = _brightness;
            //MasterVolumeSlider.Value = _masterVolume;
            //EffectsVolumeSlider.Value = _effectsVolume;
        }

        // Обработчики событий
        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow;

            // Режим окна
            if (WindowedRadioButton.IsChecked == true)
            {
                mainWindow.WindowState = WindowState.Normal;
                mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
                mainWindow.ResizeMode = ResizeMode.CanResize;
            }
            else
            {
                mainWindow.WindowState = WindowState.Maximized;
                mainWindow.WindowStyle = WindowStyle.None;
                mainWindow.ResizeMode = ResizeMode.NoResize;
            }

            MessageBox.Show("Настройки применены!", "Успех",
               MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            // Сброс к значениям по умолчанию
            ResolutionComboBox.SelectedIndex = 0;
            WindowedRadioButton.IsChecked = true;
            //BrightnessSlider.Value = 80;
            //MasterVolumeSlider.Value = 100;
            //EffectsVolumeSlider.Value = 100;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Возврат на предыдущую страницу
            if (NavigationService.CanGoBack)
            {
                NavigationService.GoBack();
            }
        }

        // Класс для хранения разрешений
        public class Resolution
        {
            public int Width { get; }
            public int Height { get; }
            public string DisplayName => $"{Width}x{Height}";

            public Resolution(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }
    }
}