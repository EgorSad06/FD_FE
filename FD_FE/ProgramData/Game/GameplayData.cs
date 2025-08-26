using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FD_FE
{
    public static class GameplayData
    {
        // режимы
        public static readonly List<GameMode> GameModes = new List<GameMode> {
            new GameMode()
            {
                fractions_count = 1,
                board_width = 4,
                board_height = 3,
                start_cards_count = 4,
                battles = 3
            },
            new GameMode()
            {
                fractions_count = 2,
                board_width = 4,
                board_height = 3,
                start_cards_count = 5,
                battles = 4
            },
            new GameMode()
            {
                fractions_count = 3,
                board_width = 5,
                board_height = 3,
                start_cards_count = 6,
                battles = 5
            },
            new GameMode()
            {
                fractions_count = 4,
                board_width = 5,
                board_height = 3,
                start_cards_count = 6,
                battles = 5
            }
        };

        // эффекты (пока без уровня эффекта)
        public static readonly List<Effect> Effects = new List<Effect>
        {
            new Effect()
            {
                name = "Заряд",
                apply = (card, targets) =>
                    card.SetAV((short)(card.AV*2))
            }
        };

        // классы гс=0 цс=1 пк=2, сз - по умолчанию
        public static readonly Dictionary<char, string> CardClassNames = new Dictionary<char, string>
        {
            { 's', "Статическое значение" },
            { 'g', "Группа союзников" },
            { 'c', "Цепь союзников" },
            { 'e', "Пустые клетки" }
        };
        public static readonly List<CardClass> CardClasses = new List<CardClass>
        {
            new CardClass() {
                id = 'g',
                GetAV = (card, board, i) => {
                    if (board.GetRDist((short)card.board_i, i) > 1) return card.AV;
                    short av = 0;
                    for (short dx=-1; dx<=1; dx++)
                        for (short dy=-1; dy<=1; dy++)
                            if (CardClass.IsAVAfct(card, board.GetCardByPos(card.board_i, dx, dy))) av++;
                    return av;
                }
            },
            new CardClass() {
                id = 'c',
                GetAV = (card, board, i) => {
                    if (board.grid[i] != null && !CardClass.IsAVAfct(card, board.grid[i])) return card.AV;
                    short av = 1;
                    short[] dir = new short[8] { 1,0,1,1,0,1,-1,1 };
                    for (int c=0; c<8; c+=2)
                    {
                        short dir_av = 1;
                        for (short dx = dir[c], dy = dir[c+1];
                        CardClass.IsAVAfct(card, board.GetCardByPos(card.board_i, dx, dy));
                        dx += dir[c], dy += dir[c+1])dir_av++;
                        for (short dx = (short)-dir[c], dy = (short)-dir[c+1];
                        CardClass.IsAVAfct(card, board.GetCardByPos(card.board_i, dx, dy));
                        dx -= dir[c], dy -= dir[c+1])dir_av++;
                        if (dir_av>av) av = dir_av;
                    }
                    return av;
                }
            },
            new CardClass() {
                id = 'e',
                GetAV = (card, board, i) => {
                    if (board.GetRDist((short)card.board_i, i) > 1) return card.AV;
                    short av = 0;
                    for (short dx=-1; dx<=1; dx++)
                        for (short dy=-1; dy<=1; dy++)
                        {
                            short cur_i = board.GetPos((short)card.board_i, dx, dy);
                            if (cur_i != -1 && board.grid[cur_i] == null) av++;
                        }
                    return av;
                }
            }
        };

        // карты 5-24 25-44 45-64 65-84
        public static int CardsPerFraction = 20;
        public static readonly Dictionary<char, List<Card>> StartCards = new Dictionary<char, List<Card>>
        {
            { 't', new List<Card> {
                new Card() {
                    id = 5,
                    name = "Рельсотрон",
                    start_HP = 4,
                    card_class = CardClasses[1],
                    image = "railgun.png",
                },

                new Card() {
                    id = 6,
                    name = "Мечтатель",
                    start_HP = 5,
                    card_class = CardClasses[2],
                    image = "dreamer.png",
                },

                new Card() {
                    id = 7,
                    name = "Хакер",
                    start_HP = 3,
                    card_class = CardClasses[0],
                    image = "hacker.png",
                },

                new Card() {
                    id = 8,
                    name = "Дрон",
                    start_HP = 2,
                    card_class = CardClasses[0],
                    image = "drone.png",
                },

                new Card() {
                    id = 9,
                    name = "Энергощит",
                    start_HP = 4,
                    card_class = new CardClass(2),
                    image = "energy_shield.png",
                },

                new Card() {
                    id = 10,
                    name = "Заводчанин",
                    start_HP = 6,
                    card_class = new CardClass(3),
                    image = "engeneer.png",
                },

                new Card() {
                    id = 11,
                    name = "Трансоформатор",
                    start_HP = 4,
                    card_class = CardClasses[0],
                    image = "transformator.png",
                },

                new Card() {
                    id = 12,
                    name = "Биомашина",
                    start_HP = 5,
                    card_class = CardClasses[0],
                    image = "biomachine.png",
                },

                new Card() {
                    id = 13,
                    name = "Кирпичемёт",
                    start_HP = 3,
                    card_class = CardClasses[1],
                    image = "brick_shooter.png",
                },

                new Card() {
                    id = 14,
                    name = "Вычислитель",
                    start_HP = 6,
                    card_class = CardClasses[1],
                    image = "calculator.png",
                }
            } },

            { 'm', new List<Card> {
                new Card() {
                    id = 25,
                    name = "Тест",
                    start_HP = 1,
                    card_class = CardClasses[0],
                    image = "transformator.png"
                }
            } },


            { 'f', new List<Card>{
                new Card() {
                    id = 45,
                    name = "Рыцарь",
                    start_HP = 6,
                    card_class = CardClasses[1],
                    image = "knight.png"
                },

                new Card() {
                    id = 46,
                    name = "Ворон",
                    start_HP = 4,
                    card_class = new CardClass(1),
                    image = "crow.png"
                },

                new Card() {
                     id=47,
                     name = "Ква-Мяу",
                     start_HP = 5,
                     card_class = CardClasses[0],
                     image = "team.png"
                },

                new Card() {
                     id=48,
                     name = "Болтник",
                     start_HP = 6,
                     card_class = CardClasses[0],
                     image = "frogmaster.png"
                },

                new Card() {
                     id=49,
                     name = "Маг",
                     start_HP = 4,
                     card_class = new CardClass(1),
                     image = "wizard.png"
                },

                new Card() {
                     id=50,
                     name = "Рояль",
                     start_HP = 4,
                     card_class = CardClasses[0],
                     image = "piano.png"
                },

                new Card() {
                     id=51,
                     name = "Жаб",
                     start_HP = 2,
                     card_class = CardClasses[0],
                     image = "frog.png"
                },

                new Card() {
                     id=52,
                     name = "Водяной",
                     start_HP = 5,
                     card_class = CardClasses[2],
                     image = "WaterMan.png"
                },

                new Card() {
                     id=53,
                     name = "Лучник",
                     start_HP = 4,
                     card_class = CardClasses[1],
                     image = "archers.png"
                }
            } }
        };
    }
}