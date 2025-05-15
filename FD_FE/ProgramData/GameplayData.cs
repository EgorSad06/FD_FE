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
        static private string _card_sprites_path_xaml = "/ClassLib;component/Assets/Sprites/Cards/";
        static public string sprites_path = "../../../ProgramData/Assets/Sprites/";
        static public List<GameMode> GameModes = new List<GameMode> {
            new GameMode()
            {
                id = 1,
                fractions_count = 1,
                board_width = 3,
                board_length = 4,
                start_cards_count = 4,
                battles = 3
            },
            new GameMode()
            {
                id = 2,
                fractions_count = 2,
                board_width = 3,
                board_length = 4,
                start_cards_count = 5,
                battles = 4
            },
            new GameMode()
            {
                id = 3,
                fractions_count = 3,
                board_width = 4,
                board_length = 4,
                start_cards_count = 6,
                battles = 5
            },
            new GameMode()
            {
                id = 4,
                fractions_count = 4,
                board_width = 4,
                board_length = 4,
                start_cards_count = 6,
                battles = 5
            }

        };
        static public List<Effect> FD_FE_Effects = new List<Effect>
        {
            new Effect()
            {
                id = 1,
                name = "Charge",
                function = delegate(BoardCard card)
                {
                    card.AV = (short)(card.AV*2);
                }
            }
        };
        static public List<CardClass> CardClasses = new List<CardClass>
        {
            new CardClass()
            {
                id = 'e',
                name = "Static value",
            },
            new CardClass()
            {
                id = 'g',
                name = "Group of teammates",
                GetAV = delegate(BoardCard card)
                {
                    return 1;
                }
            },

        };
        static public List<Card> StartCards = new List<Card>
        {
            new Card()
            {
                id = 5,
                name = "Railgun",
                fraction = 't',
                card_class = 'g',
                image = "railgun.png",
                function = delegate() {

                }
            },
            new Card()
            {
                id = 6,
                name = "Dreamer",
                fraction = 't',
                card_class = 'e',
                image = "dreamer.png",
                function = delegate()
                {

                }
            },
            new Card()
            {
                id = 7,
                name = "Hacker",
                fraction = 't',
                card_class = 'g',
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