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

        // ������
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

        // �������
        public static readonly List<Effect> FD_FE_Effects = new List<Effect>
        {
            new Effect()
            {
                name = "�����",
                function = delegate(BoardCard card)
                {
                    card.SetAV((short)(card.AV*2));
                }
            }
        };

        // ������ ��=0 ��=1 ��=2 ��=3 ������������������=4
        public static readonly List<CardClass> CardClasses = new List<CardClass>
        {
            new CardClass() {
                id = 's',
                name = "����������� ��������"
            },
            new CardClass() {
                id = 'g',
                name = "������ ���������",
                GetAV = delegate(BoardCard card) { return 1; }
            },
            new CardClass() {
                id = 'c',
                name = "���� ���������",
                GetAV = delegate(BoardCard card) { return 1; }
            },
            new CardClass() {
                id = 'e',
                name = "������ ������",
                GetAV = delegate(BoardCard card) { return 1; }
            },
            new CardClass()
            {
                id='d',
                name= "������������������",
                GetAV = delegate(BoardCard card) {return 1;  }
            }
        };

        // ����� 5-24 25-44 45-64 65-84
        public static readonly Dictionary<char, List<Card>> StartCards = new Dictionary<char, List<Card>>
        {
            { 't', new List<Card> {
                new Card()
                {
                    id = 5,
                    name = "����������",
                    card_class = CardClasses[1],
                    image = "railgun.png",
                    select_n = 1,
                    function = (BoardCard card)=> {

                    }
                },
                new Card()
                {
                    id = 6,
                    name = "���������",
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
                    name = "�����",
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
                    name = "����",
                    card_class = CardClasses[0],
                    image = "transformator.png"
                }
            } },
            { 'f', new List<Card>{
                new Card()
                {
                    id = 45,
                    name = "������",
                    card_class = CardClasses[4],
                    image = "knight.png"
                },
                new Card()
                {
                    id = 46,
                    name = "�����",
                    card_class = CardClasses[0],
                    image = "crow.png"
                },
                new Card()
                {
                     id=47,
                     name = "���-���",
                     card_class = CardClasses[1],
                     image = "team.png"
                },
                new Card()
                {
                     id=48,
                     name = "�������",
                     card_class = CardClasses[1],
                     image = "frogmaster.png"
                },
                new Card()
                {
                     id=49,
                     name = "���",
                     card_class = CardClasses[0],
                     image = "wizard.png"
                },
                new Card()
                {
                     id=50,
                     name = "�����",
                     card_class = CardClasses[1],
                     image = "piano.png"
                },
                new Card()
                {
                     id=51,
                     name = "���",
                     card_class = CardClasses[1],
                     image = "ftog.png"
                },
                new Card()
                {
                     id=52,
                     name = "�������",
                     card_class = CardClasses[3],
                     image = "WaterMan.png"
                },
                new Card()
                {
                     id=53,
                     name = "������",
                     card_class = CardClasses[2],
                     image = "archers.png"
                }
            } }
        };
    }
}