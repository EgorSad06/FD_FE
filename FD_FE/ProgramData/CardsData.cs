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
    public static class CardsData
    {
        static private string _card_sprites_path = "/ClassLib;component/Assets/Sprites/Cards/";
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
        static public List<Card> StartCards = new List<Card>
        {
            new Card()
            {
                id = 5,
                name = "Railgun",
                image = _card_sprites_path+"railgun.png",
                function = delegate() {

                }
            },
            new Card()
            {

            }
        };
        static public void add_cards() { }
    }
}