using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
// режим
    public class GameMode
    {
        public short fractions_count { get; set; }
        public short board_width { get; set; }
        public short board_length { get; set; }
        public short start_cards_count { get; set; }
        public short battles { get; set; }
    }

    public delegate void Card_func(BoardCard card);
    public delegate short GetAV_func(BoardCard card);
    

// эффект
    public class Effect
    {
        public string name { get; set; }
        public Card_func function { get; set; }
    }

// класс карты
    public class CardClass
    {
        public char id { get; set; }
        public string name { get; set; }
        public GetAV_func GetAV { get; set; }

    }

// карта
    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CardClass card_class { get; set; }
        public short start_HP { get; set; }
        public short select_n { get; set; }
        public Card_func function { get; set; }
        public string image { get; set; }

        public Card() { }
        public Card(Card card) // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            start_HP = card.start_HP; function = card.function; image = card.image;
        }
        public char GetFraction() => GameplayData.StartCards.Keys.ElementAt( (id - GameplayData.StartCards.Values.ElementAt(0)[0].id) / 20 );
        static public char GetFraction(int id) => GameplayData.StartCards.Keys.ElementAt((id - GameplayData.StartCards.Values.ElementAt(0)[0].id) / 20);
    }
    
// карта поля
    public class BoardCard : Card
    {
        public int board_i { get; set; }
        public Card source { get; set; }
        public char force { get; set; }
        public short HP { get; protected set; }
        public short AV { get; protected set; }
        public void SetHP(short new_HP) { HP = new_HP; CardChanged?.Invoke(); }
        public void SetAV() { AV = card_class.GetAV(this); CardChanged?.Invoke(); }
        public void SetAV(short new_AV) { AV = new_AV; CardChanged?.Invoke(); }

        public List<Effect> effects { get; set; }

        public delegate void CardChangedEventHandler();
        public event CardChangedEventHandler CardChanged;

        public BoardCard() { }
        public BoardCard(Card card, int board_index = 0) // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            start_HP = card.start_HP; function = card.function; image = card.image;
            source = card;
            board_i = board_index;
        }

        public void Act()
        {
            function(this);
        }
        public void SetAct() { }
    }

// колода
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
        public bool SqncEnd() => _slcti < _sequence.Count;
        public Card GetCard() => _sequence[_slcti++];
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
            _slcti = 0;
        }
    }

// поле
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
