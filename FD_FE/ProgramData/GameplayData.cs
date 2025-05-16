using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FD_FE
{
    public static class GameplayData
    {
        static public string sprites_path = "../../../ProgramData/Assets/Sprites/";

        // режимы
        public static List<GameMode> GameModes = new List<GameMode> {
            new GameMode()
            {
                fractions_count = 1,
                board_width = 3,
                board_length = 4,
                start_cards_count = 4,
                battles = 3
            },
            new GameMode()
            {
                fractions_count = 2,
                board_width = 3,
                board_length = 4,
                start_cards_count = 5,
                battles = 4
            },
            new GameMode()
            {
                fractions_count = 3,
                board_width = 4,
                board_length = 4,
                start_cards_count = 6,
                battles = 5
            },
            new GameMode()
            {
                fractions_count = 4,
                board_width = 4,
                board_length = 4,
                start_cards_count = 6,
                battles = 5
            }
        };

        // эффекты
        static public List<Effect> FD_FE_Effects = new List<Effect>
        {
            new Effect()
            {
                name = "Заряд",
                function = delegate(BoardCard card)
                {
                    card.SetAV((short)(card.AV*2));
                }
            }
        };

        // классы сз=0 гс=1 цс=2 пк=3
        static public List<CardClass> CardClasses = new List<CardClass>
        {
            new CardClass() {
                id = 's',
                name = "Статическое значение"
            },
            new CardClass() {
                id = 'g',
                name = "Группа союзников",
                GetAV = delegate(BoardCard card) { return 1; }
            },
            new CardClass() {
                id = 'c',
                name = "Цепь союзников",
                GetAV = delegate(BoardCard card) { return 1; }
            },
            new CardClass() {
                id = 'e',
                name = "Пустые клетки",
                GetAV = delegate(BoardCard card) { return 1; }
            }
        };

        // карты 5-24 25-44 45-64 65-84
        static public List<Card> StartCards = new List<Card>
        {
            new Card()
            {
                id = 5,
                name = "Рельсотрон",
                fraction = 't',
                card_class = CardClasses[1],
                image = "railgun.png",
                function = delegate() {

                }
            },
            new Card()
            {
                id = 6,
                name = "Мечтатель",
                fraction = 't',
                card_class = CardClasses[3],
                image = "dreamer.png",
                function = delegate()
                {

                }
            },
            new Card()
            {
                id = 7,
                name = "Хакер",
                fraction = 't',
                card_class = CardClasses[1],
                image = "hacker.png",
                function = delegate()
                {

                }
            }
            //new Card()
            //{
            //    id = 45,
            //    name = "Knight",
            //    fraction = 'f',
            //    card_class = 'c',
            //    image = "hacker.png",
            //    function = delegate()
            //    {

            //    }
            //}

        };
    }
}