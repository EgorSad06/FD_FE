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
    public partial class TutorialPage : Page, INotifyPropertyChanged
    {
        private int _currentIndex;
        private List<Card> _cards = new List<Card>();
        private Card _currentCard;

        public List<Card> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged();
            }
        }

        public Card CurrentCard
        {
            get => _currentCard;
            set
            {
                // Сбрасываем выделение предыдущей карточки
                if (_currentCard != null)
                    _currentCard.IsSelected = false;

                _currentCard = value;

                // Устанавливаем выделение новой карточки
                if (_currentCard != null)
                    _currentCard.IsSelected = true;

                OnPropertyChanged();
                UpdateVisibility(); // Добавлено обновление видимости
            }
        }

        // Команды с явным обновлением CurrentCard
        public ICommand NextCommand => new RelayCommand(() =>
        {
            if (_currentIndex < _cards.Count - 1)
            {
                _currentIndex++;
                CurrentCard = _cards[_currentIndex]; // Явное обновление
            }
        });

        public ICommand PreviousCommand => new RelayCommand(() =>
        {
            if (_currentIndex > 0)
            {
                _currentIndex--;
                CurrentCard = _cards[_currentIndex]; // Явное обновление
            }
        });

        public ICommand ReturnCommand => new RelayCommand(() =>
        {
            NavigationService.Navigate(new MainMenu());
        });

        private void UpdateVisibility()
        {
            // Принудительное обновление свойств видимости
            OnPropertyChanged(nameof(ShowBackButton));
            OnPropertyChanged(nameof(ShowNextButton));
            OnPropertyChanged(nameof(ShowReturnButton));
        }

        public bool ShowBackButton => _currentIndex > 0;
        public bool ShowNextButton => _currentIndex < _cards.Count - 1;
        public bool ShowReturnButton => _currentIndex == _cards.Count - 1;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public class Card : INotifyPropertyChanged
        {
            private bool _isSelected;
            private BitmapImage _image;
            private string _title;
            private string _description;

            public bool IsSelected
            {
                get => _isSelected;
                set
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }

            public BitmapImage Image
            {
                get => _image;
                set
                {
                    _image = value;
                    OnPropertyChanged();
                }
            }

            public string Title
            {
                get => _title;
                set
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }

            public string Description
            {
                get => _description;
                set
                {
                    _description = value;
                    OnPropertyChanged();
                }
            }

            public Card(string title, string imageName, string desc)
            {
                Title = title;
                Image = LoadImage(imageName);
                Description = desc;
                IsSelected = false; // Инициализация значения
            }

            private BitmapImage LoadImage(string filename)
            {
                try
                {
                    return new BitmapImage(new Uri(
                        $"Assets/Sprites/Card/{filename}"));
                }
                catch
                {
                    return new BitmapImage();
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string prop = "") =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public TutorialPage()
        {
            InitializeComponent();
            DataContext = this;

            // Инициализация карточек с явным указанием IsSelected для первой
            Cards = new List<Card>
            {
                new Card("Здоровье", "biomachine.png", "Тут, что-то будет") { IsSelected = true },
                new Card("Класс", "brick_shooter.png", "Тут, что-то будет"),
                new Card("Активное значение", "hacker.png", "Тут, что-то будет"),
                new Card("Завершение", "transformator.png", "Тут, что-то будет")
            };

            _currentIndex = 0;
            CurrentCard = Cards[0];
        }

        public class RelayCommand : ICommand
        {
            private readonly Action _execute;

            public RelayCommand(Action execute) => _execute = execute;

            public bool CanExecute(object parameter) => true;
            public void Execute(object parameter) => _execute();
            public event EventHandler CanExecuteChanged;
        }
    }
}