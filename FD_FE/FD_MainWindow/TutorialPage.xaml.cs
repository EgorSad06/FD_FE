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
        private List<BoardCard> _cards;
        private BitmapImage _currentCardImage;

        public BoardCard CurrentCard => _cards?[_currentIndex];
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
            _cards = new List<BoardCard>
        {
            new BoardCard
            {
                name = "Здоровье",
                description = "Текст с описанием 1",
                image = "biomachine.png",
                fraction = 'g',
                card_class = GameplayData.CardClasses[0]
            },
            new BoardCard
            {
                name = "Класс",
                description = "Текст с описанием 2",
                image = "brick_shooter.png",
                fraction = 'g',
                card_class = GameplayData.CardClasses[0]
            },
            new BoardCard
            {
                name = "Особое значение",
                description = "Текст с описанием 3",
                image = "hacker.png",
                fraction = 'g',
                card_class = GameplayData.CardClasses[0]
            }
        };
        }

        private void UpdateImage()
        {
            UCCard uc_card = Game.Draw(CurrentCard, TutorialGrid, 0, 0);
            uc_card.HorizontalAlignment = HorizontalAlignment.Right;
            uc_card.VerticalAlignment = VerticalAlignment.Center;
            uc_card.cardVB.Height *= 3;
            uc_card.cardVB.Width *= 3;
            uc_card.IsEnabled = false; // чтобы карта не нажималась

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