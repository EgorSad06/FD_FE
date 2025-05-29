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
        public short board_height { get; set; }
        public short start_cards_count { get; set; }
        public short battles { get; set; }
    }

    public delegate void Card_func(BoardCard card, BoardCard[] targets);
    public delegate short GetV_func(BoardCard card);
    

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
        public GetV_func GetAV { get; set; }

    }

// карта
    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CardClass card_class { get; set; }
        public short start_HP { get; set; }
        public GetV_func select_n { get; set; }
        public Card_func function { get; set; }
        public string image { get; set; }

        protected Card() { }
        public Card(GetV_func sn_func, Card_func func) { select_n = sn_func; function = func; }
        public Card(Card card) // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            start_HP = card.start_HP; function = card.function; image = card.image;
            select_n = card.select_n;
            function = card.function;
        }
        public char GetFraction() => GameplayData.StartCards.Keys.ElementAt( (id - GameplayData.StartCards.Values.ElementAt(0)[0].id) / 20 );
        static public char GetFraction(int id) => GameplayData.StartCards.Keys.ElementAt((id - GameplayData.StartCards.Values.ElementAt(0)[0].id) / 20);
    }
    
// карта поля
    public class BoardCard : Card
    {
        public int board_i { get; protected set; }
        public Card source { get; set; }
        public char force { get; set; }
        public short HP { get; protected set; }
        public short AV { get; protected set; }
        public bool card_act { get; set; } = true;
        public void SetBI(int new_board_i) {
            int prev_i = board_i;
            board_i = new_board_i;
            CardMoved?.Invoke(this, prev_i);
        }
        public void SetHP(short new_HP) { HP = new_HP; CardChanged?.Invoke(this); }
        public void SetAV() { AV = card_class.GetAV(this); CardChanged?.Invoke(this); }
        public void SetAV(short new_AV) { AV = new_AV; CardChanged?.Invoke(this); }

        public List<Effect> effects { get; set; }

        public delegate void CardChangedEventHandler(BoardCard sender);
        public delegate void CardMovedEventHandler(BoardCard sender, int prev_i);
        public event CardChangedEventHandler CardChanged;
        public event CardMovedEventHandler CardMoved;

        public BoardCard() { }
        public BoardCard(Card card, int board_index = 0, char card_force='p') // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            HP = start_HP = card.start_HP; function = card.function; image = card.image; AV = 1;
            source = card;
            force = card_force;
            board_i = board_index;
            select_n = card.select_n;
            function = card.function;
        }

        public void Act(BoardCard[] targets)
        {
            function(this, targets);
        }
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
        public Card GetCard() => _sequence[_slcti++];
        public Card MoveToHand(Card card) { hand_cards.Add(card); return card; }
        public Card MoveToHand()
        {
            Card card = GetCard();
            hand_cards.Add(card);
            return card;
        }
        public bool SqncEnd() => _slcti < _sequence.Count;
        public int SetSqnc() // установка очереди
        {
            Random rnd = new Random();
            int seed = rnd.Next();
            rnd = new Random(seed);
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
            return seed;
        }
        public int SetSqnc(int seed) // установка очереди
        {
            Random rnd;
            if (seed == 0)
            {
                rnd = new Random();
                seed = rnd.Next();
            }
            rnd = new Random(seed);
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
            return seed;
        }
    }

    // поле
    public class Board
    {
        public BoardCard[] grid { get; private set; } = null;
        //public List<BoardCard> = new List<BoardCard>;
        public short width;
        public short height;
        public readonly short count;
        public void set_grid(int i, BoardCard new_card) {
            grid[i] = new_card;
            grid[i]?.SetBI(i);
            BoardChanged?.Invoke(this, i);
        }

        public delegate void BoardChangedEventHandler(Board sender, int grid_i);
        public BoardChangedEventHandler BoardChanged;

        public Board() { }
        public Board(short width, short height)
        {
            this.width = width;
            this.height = height;
            count = (short)(width*height);
            grid = new BoardCard[count];
        }

        public void AddBoardCard(Card new_card)
        {
            BoardCard new_bcard = null;
            int i = 0;
            while (i < count && grid[i] != null) i++;
            if (new_card != null && i != count) {
                set_grid(i, new_bcard = new BoardCard(new_card, i));
                new_bcard.CardChanged += BoardCardChanged;
                new_bcard.CardMoved += BoardCardMoved;
            }
        }
        public void SetBoardCard(Card new_card, int i, char force='p')
        {
            set_grid(i, (new_card != null) ? new BoardCard(new_card, i, force) : null);
        }
        public void SetBoardCard(BoardCard new_card, int i)
        {
            set_grid(i, new_card);
        }
        public BoardCard RemBoardCard(int i)
        {
            if (i >= count) return null;
            BoardCard t = grid[i];
            set_grid(i, null);
            t.CardChanged -= BoardCardChanged;
            t.CardMoved -= BoardCardMoved;
            t.SetBI(-1);
            return t;
        }

        public void BoardCardChanged(BoardCard sender)
        {
            if (sender.HP < 0) {
                
                RemBoardCard(sender.board_i);
            }
        }
        public void BoardCardMoved(BoardCard sender, int prev_i)
        {
            set_grid(sender.board_i, sender);
            set_grid(prev_i, null);
        }
    }
}
