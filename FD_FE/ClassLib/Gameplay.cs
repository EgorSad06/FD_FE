using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace FD_FE
{
    public delegate void Card_func();
    public delegate void Efct_func();

    public class Effect
    {
        public int id;
        Efct_func function;
    }

    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public char fraction { get; set; }
        public char card_class { get; set; }
        public short start_HP { get; set; }
        public Card_func function { get; set; }
        public string image { get; set; }
    }

    public class Deck
    {
        public List<Card> deck_cards;
        public List<Card> hand_cards;
        private List<Card> _sequence;
        private int _si = 0;

        public Card GetCard() { return _sequence[_si++]; }
        public Card GetCard(int i) { return deck_cards[i]; }
        public void MoveToHand(Card card) { hand_cards.Add(card); }

    }

    public class BoardCard : Card
    {
        public char force { get; set; }
        public short HP { get; set; }
        public short AV { get; set; }
        public Effect[] effects { get; set; }
        public void Act()
        {

        }
        public void SetAct() { }
        public void Draw() { }
    }

    public class Board
    {
        public List<BoardCard> grid;

    }
}
