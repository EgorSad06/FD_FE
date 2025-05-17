using FD_FE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace FD_MainWindow
{
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        public RelayCommand(Action execute) => _execute = execute;

        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute();
        public event EventHandler CanExecuteChanged;
    }
    public partial class TutorialPage : Page, INotifyPropertyChanged
    {
        private int _currentIndex;
        private List<Card> _cards;
        private BitmapImage _currentCardImage;

        public Card CurrentCard => _cards?[_currentIndex];
        public BitmapImage CurrentCardImage
        {
            get => _currentCardImage;
            set
            {
                _currentCardImage = value;
                OnPropertyChanged();
            }
        }

        public TutorialPage()
        {
            InitializeComponent();
            DataContext = this;
            LoadCards();
            UpdateImage();
        }

        private void LoadCards()
        {
            // Загрузка карточек из источника
            _cards = new List<Card>
        {
            new Card
            {
                name = "Здоровье",
                description = "Текст с описанием 1",
                image = "biomachine.png"
            },
            new Card
            {
                name = "Класс",
                description = "Текст с описанием 2",
                image = "brick_shooter.png"
            },
            new Card
            {
                name = "Особое значение",
                description = "Текст с описанием 3",
                image = "hacker.png"
            }
        };
        }

        private void UpdateImage()
        {
            try
            {
                
                string uriPath = $"pack://application:,,,/FD_MainWindow;component/ProgramData/Assets/Sprites/Cards/{CurrentCard.image}";
                CurrentCardImage = new BitmapImage(new Uri(uriPath, UriKind.Absolute));
            }
            catch (Exception ex)
            {
                // Обработка ошибок
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                CurrentCardImage = new BitmapImage(); // Пустое изображение при ошибке
            }

            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(ShowBackButton));
            OnPropertyChanged(nameof(ShowNextButton));
        }

        public ICommand NextCommand => new RelayCommand(() =>
        {
            if (_currentIndex < _cards.Count - 1)
            {
                _currentIndex++;
                UpdateImage();
            }
        });

        public ICommand PreviousCommand => new RelayCommand(() =>
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                UpdateImage();
            }
        });

        public ICommand ReturnCommand => new RelayCommand(() =>
        {
            NavigationService.Navigate(new MainMenu());
        });

        public bool ShowBackButton => _currentIndex > 0;
        public bool ShowNextButton => _currentIndex < _cards.Count - 1;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}