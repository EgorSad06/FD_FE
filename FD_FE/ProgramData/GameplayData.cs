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
                board_width = 4,
                board_height = 4,
                start_cards_count = 6,
                battles = 5
            },
            new GameMode()
            {
                fractions_count = 4,
                board_width = 4,
                board_height = 4,
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
                function = delegate(BoardCard card, BoardCard[] targets)
                {
                    card.SetAV((short)(card.AV*2));
                }
            }
        };

        // ������ ��=0 ��=1 ��=2 ��=3
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
            }
            //new CardClass()
            //{
            //    id='d',
            //    name= "������������������",
            //    GetAV = delegate(BoardCard card) {return 1;  }
            //}
        };

        // ����� 5-24 25-44 45-64 65-84
        public static readonly Dictionary<char, List<Card>> StartCards = new Dictionary<char, List<Card>>
        {
            { 't', new List<Card> {
                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{
                        
                    }
                ) {
                    id = 5,
                    name = "����������",
                    start_HP = 4,
                    card_class = CardClasses[2],
                    image = "railgun.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 6,
                    name = "���������",
                    start_HP = 5,
                    card_class = CardClasses[3],
                    image = "dreamer.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 7,
                    name = "�����",
                    start_HP = 3,
                    card_class = CardClasses[1],
                    image = "hacker.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 8,
                    name = "����",
                    start_HP = 2,
                    card_class = CardClasses[1],
                    image = "drone.png",
                },

                new Card(
                    (BoardCard card)=>{ return 0; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 9,
                    name = "���������",
                    start_HP = 4,
                    card_class = CardClasses[0],
                    image = "energy_shield.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 10,
                    name = "����������",
                    start_HP = 6,
                    card_class = CardClasses[0],
                    image = "engeneer.png",
                },

                new Card(
                    (BoardCard card)=>{ return card.AV; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 11,
                    name = "��������������",
                    start_HP = 4,
                    card_class = CardClasses[1],
                    image = "transformator.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 12,
                    name = "���������",
                    start_HP = 5,
                    card_class = CardClasses[1],
                    image = "biomachine.png",
                },

                new Card(
                    (BoardCard card)=>{ return card.AV; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 13,
                    name = "���������",
                    start_HP = 3,
                    card_class = CardClasses[2],
                    image = "brick_shooter.png",
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 14,
                    name = "�����������",
                    start_HP = 6,
                    card_class = CardClasses[2],
                    image = "calculator.png",
                }
            } },

            //{ 'm', new List<Card> {
            //    new Card(
            //        (BoardCard card)=>{ return 1; },
            //        (BoardCard card, BoardCard[] targets)=>{

            //        }
            //    ) {
            //        id = 25,
            //        name = "����",
            //        start_HP = 1,
            //        card_class = CardClasses[0],
            //        image = "transformator.png"
            //    }
            //} },

            { 'f', new List<Card>{
                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 45,
                    name = "������",
                    card_class = CardClasses[0],
                    image = "knight.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                    id = 46,
                    name = "�����",
                    card_class = CardClasses[0],
                    image = "crow.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=47,
                     name = "���-���",
                     card_class = CardClasses[1],
                     image = "team.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=48,
                     name = "�������",
                     card_class = CardClasses[1],
                     image = "frogmaster.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=49,
                     name = "���",
                     card_class = CardClasses[0],
                     image = "wizard.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=50,
                     name = "�����",
                     card_class = CardClasses[1],
                     image = "piano.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=51,
                     name = "���",
                     card_class = CardClasses[1],
                     image = "frog.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=52,
                     name = "�������",
                     card_class = CardClasses[3],
                     image = "WaterMan.png"
                },

                new Card(
                    (BoardCard card)=>{ return 1; },
                    (BoardCard card, BoardCard[] targets)=>{

                    }
                ) {
                     id=53,
                     name = "������",
                     card_class = CardClasses[2],
                     image = "archers.png"
                }
            } }
        };
    }
}