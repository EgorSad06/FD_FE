using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FD_FE
{
    public delegate void Card_func();
    public delegate void Efct_func(BoardCard card);
    static public class IBoardAct
    {
        static public BoardCard SelectCard() {
            return new BoardCard(new Card());
        }
    }
    public class Effect
    {
        public int id { get; set; }
        public string name { get; set; }
        public Efct_func function { get; set; }
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

        public Card() { }
        public Card(Card card) // копия имеющейся карты
        { id = card.id; name = card.name; fraction = card.fraction; card_class = card.card_class; start_HP = card.start_HP; function = card.function; image = card.image; }
    }

    public class BoardCard : Card
    {
        public char force { get; set; }
        public short HP { get; set; }
        public short AV { get; set; }
        public List<Effect> effects { get; set; }

        public BoardCard(Card card) // копия имеющейся карты
        { id = card.id; name = card.name; fraction = card.fraction; card_class = card.card_class; start_HP = card.start_HP; function = card.function; image = card.image; }

        public void Act()
        {

        }
        public void SetAct() { }
    }
    public class Deck
    {
        public List<Card> deck_cards = new List<Card>();
        public List<Card> hand_cards = new List<Card>();
        private List<Card> _sequence = new List<Card>();
        private int _slcti = 0;

        public Deck() { }
        public Deck(List<Card> cards)
        {
            for (int i=0; i<cards.Count; i++) { deck_cards.Add(new Card(cards[i])); }
        }

        public Card GetCard() { return (_slcti<_sequence.Count) ? _sequence[_slcti++] : null; }
        public Card GetCard(int i) { return deck_cards[i]; }
        public void MoveToHand(Card card) { hand_cards.Add(card); }
        public void SetSqnc()
        {
            Random rnd = new Random();
            int n = deck_cards.Count;
            _sequence.AddRange(deck_cards);
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n);
                Card temp = _sequence[k];
                _sequence[k] = _sequence[n];
                _sequence[n] = temp;
            }
        }
    }

    public class Board
    {
        public List<BoardCard> grid = new List<BoardCard>();

        public void SetBoardCard(Card new_card)
        {
            grid.Add(new BoardCard(new_card));
        }
        public void SetBoardCard(Card new_card, int i)
        {
            for (int j = grid.Count; j <= i; j++) { grid.Add(null); }
            grid[i] = new BoardCard(new_card);
        }
    }
}
