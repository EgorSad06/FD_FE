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
        static public List<Style> Styles = new List<Style> {
            new Style() {

            }
        };
        static public List<Card> StartCards = new List<Card>
        {
            new Card()
            {
                id = 5,
                name = "Railgun",
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