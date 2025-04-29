using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FD_FE
{
    static class CardsData
    {
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