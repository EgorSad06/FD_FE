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
        public static readonly List<GameMode> GameModes = new List<GameMode> {
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
        public static readonly List<Effect> FD_FE_Effects = new List<Effect>
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

        // классы сз=0 гс=1 цс=2 пк=3 самоуничтожающийся=4
        public static readonly List<CardClass> CardClasses = new List<CardClass>
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
            },
            new CardClass()
            {
                id='d',
                name= "самоуничтожающийся",
                GetAV = delegate(BoardCard card) {return 1;  }
            }
        };

        // карты 5-24 25-44 45-64 65-84
        public static readonly Dictionary<char, List<Card>> StartCards = new Dictionary<char, List<Card>>
        {
            { 't', new List<Card> {
                new Card()
                {
                    id = 5,
                    name = "Рельсотрон",
                    card_class = CardClasses[1],
                    image = "railgun.png",
                    select_n = 1,
                    function = (BoardCard card)=> {

                    }
                },
                new Card()
                {
                    id = 6,
                    name = "Мечтатель",
                    card_class = CardClasses[3],
                    image = "dreamer.png",
                    select_n = 1,
                    function = (BoardCard card)=>
                    {

                    }
                },
                new Card()
                {
                    id = 7,
                    name = "Хакер",
                    card_class = CardClasses[1],
                    image = "hacker.png",
                    function = (BoardCard card)=>
                    {
                        card.select_n++;
                    }
                }
            } },
            { 'm', new List<Card> {
                new Card()
                {
                    id = 25,
                    name = "Тест",
                    card_class = CardClasses[0],
                    image = "transformator.png"
                }
            } },
            { 'f', new List<Card>{
                new Card()
                {
                    id = 45,
                    name = "Рыцарь",
                    card_class = CardClasses[4],
                    image = "knight.png"
                },
                new Card()
                {
                    id = 46,
                    name = "Ворон",
                    card_class = CardClasses[0],
                    image = "crow.png"
                },
                new Card()
                {
                     id=47,
                     name = "Ква-Мяу",
                     card_class = CardClasses[1],
                     image = "team.png"
                },
                new Card()
                {
                     id=48,
                     name = "Болтник",
                     card_class = CardClasses[1],
                     image = "frogmaster.png"
                },
                new Card()
                {
                     id=49,
                     name = "Маг",
                     card_class = CardClasses[0],
                     image = "wizard.png"
                },
                new Card()
                {
                     id=50,
                     name = "Рояль",
                     card_class = CardClasses[1],
                     image = "piano.png"
                },
                new Card()
                {
                     id=51,
                     name = "Жаб",
                     card_class = CardClasses[1],
                     image = "ftog.png"
                },
                new Card()
                {
                     id=52,
                     name = "Водяной",
                     card_class = CardClasses[3],
                     image = "WaterMan.png"
                },
                new Card()
                {
                     id=53,
                     name = "Лучник",
                     card_class = CardClasses[2],
                     image = "archers.png"
                }
            } }
        };
    }
}