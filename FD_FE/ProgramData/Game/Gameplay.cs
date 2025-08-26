using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
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

    public delegate void TargetAction(BoardCard card, BoardCard[] targets);
    public delegate short CardValue(BoardCard card);
    public delegate short AdvCardValue(BoardCard card, Board board, short grid_i);
    public delegate short ShortFunction();

// эффект
    public class Effect
    {
        public string name { get; set; }
        public TargetAction apply { get; set; }
    }

// класс карты
    public class CardClass
    {
        public char id = 's';
        public AdvCardValue GetAV;
        public CardClass() { }
        public CardClass(short static_av) => GetAV = (c, b, i) => static_av;
        public static bool IsAVAfct(BoardCard card1, BoardCard card2) => card1.force == card2?.force && card1.card_class.id == card2?.card_class.id;
    }

    // карта
    public class Card
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public CardClass card_class { get; set; }
        public short start_HP { get; set; }
        public ShortFunction select_n { get; set; } = () => { return 1; };
        public TargetAction function { get; set; } = (card, targets) => { if (targets.Length > 0) targets[0]?.AfctHP((short)-card.AV); };
        public string image { get; set; }

        public Card() { }
        public Card(Card card) // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            start_HP = card.start_HP; function = card.function; image = card.image;
            select_n = card.select_n;
            function = card.function;
        }
    }
    
// карта поля
    public class BoardCard : Card
    {
        public int board_i { get; internal set; }
        public char force;
        public short HP { get; protected set; }
        public short AV { get; protected set; }
        public short last_act_clr = 0;
        public void Move(int new_board_i) {
            int prev_i = board_i;
            board_i = new_board_i;
            CardMoved?.Invoke(this, prev_i);
        }
        public short SetHP(short new_HP)
        {
            short t = HP;
            HP = new_HP;
            LocalCardChanged?.Invoke(this);
            CardChanged?.Invoke(this);
            return t;
        }
        public short AfctHP(short d_HP) => SetHP((short)Math.Max(HP + d_HP, 0));
        public void SetAV(short new_AV) { AV = new_AV; LocalCardChanged?.Invoke(this); }
        public void SetAV(Board board, int grid_i) => SetAV(card_class.GetAV(this, board, (short)grid_i));
        public List<Effect> effects { get; set; }

        public delegate void CardChangedEventHandler(BoardCard sender);
        public static event CardChangedEventHandler CardChanged;
        public event CardChangedEventHandler LocalCardChanged;
        public event CardChangedEventHandler CardStartedAct;
        public event CardChangedEventHandler CardEndedAct;
        public delegate void CardMovedEventHandler(BoardCard sender, int prev_i);
        public event CardMovedEventHandler CardMoved;

        public BoardCard() { }
        public BoardCard(Card card, int board_index = 0, char card_force='p') // копия имеющейся карты
        {
            id = card.id; name = card.name; description = card.description; card_class = card.card_class;
            HP = start_HP = card.start_HP; function = card.function; image = card.image;
            AV = card_class.id == 's' ? card_class.GetAV(null,null,-1) : (short)1;
            force = card_force;
            board_i = board_index;
            select_n = card.select_n;
            function = card.function;
        }

        public async Task Act(BoardCard[] targets, int wait=0)
        {
            wait /= 2;
            CardStartedAct.Invoke(this);
            await Task.Delay(wait);
            function(this, targets);
            await Task.Delay(wait);
            CardEndedAct.Invoke(this);
        }
    }

// колода
    public class Deck
    {
        public List<Card> deck_cards;
        public List<Card> hand_cards = new List<Card>();
        private List<Card> _sequence;
        private int _slcti = 0;

        public Deck() => deck_cards = new List<Card>();
        public Deck(List<Card> cards) => deck_cards = new List<Card>(cards);
        public Card NextCard() => _sequence[_slcti++];
        public Card AddToHand(Card card)
        {
            for (int i = 0; i < hand_cards.Count; i++)
                if (hand_cards[i] == null)
                    return hand_cards[i] = card;
            hand_cards.Add(card);
            return card;
        }
        public Card NextToHand()
        {
            Card card = NextCard();
            for (int i=0; i<hand_cards.Count; i++)
                if (hand_cards[i] == null)
                    return hand_cards[i] = card;
            hand_cards.Add(card);
            return card;
        }
        public Card RemFromHand(short i)
        {
            Card card = hand_cards[i];
            hand_cards[i] = null;
            return card;
        }
        public bool SqncEnd() => _slcti < _sequence.Count;
        public int SetSqnc() // установка очереди
        {
            int seed = new Random().Next();
            Random rnd = new Random(seed);
            int n = deck_cards.Count;
            _sequence = new List<Card>();
            _sequence.AddRange(deck_cards);
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n);
                (_sequence[n], _sequence[k]) = (_sequence[k], _sequence[n]);
            }
            _slcti = 0;
            return seed;
        }
        public int SetSqnc(int seed) // установка очереди
        {
            if (seed == 0) seed = new Random().Next();
            Random rnd = new Random(seed);
            int n = deck_cards.Count;
            _sequence = new List<Card>();
            _sequence.AddRange(deck_cards);
            while (n > 1)
            {
                n--;
                int k = rnd.Next(n);
                (_sequence[n], _sequence[k]) = (_sequence[k], _sequence[n]);
            }
            _slcti = 0;
            return seed;
        }
    }

// поле
    public class Board
    {
        public readonly bool IsPlayboard;
        public BoardCard[] grid { get; private set; } = null;
        public readonly short width;
        public readonly short height;
        public readonly short count;
        private void SetGridCard(int i, BoardCard new_card) {
            if (new_card != null)
            {
                new_card.board_i = i;
                new_card.LocalCardChanged += OnBoardCardChanged;
                new_card.CardMoved += OnBoardCardMoved;
                if (IsPlayboard) BoardChanged += new_card.SetAV;
            }
            grid[i] = new_card;
            BoardChanged?.Invoke(this, i);
        }

        public delegate void BoardChangedEventHandler(Board sender, int grid_i);
        public event BoardChangedEventHandler BoardChanged;

        public Board() { }
        public Board(short width, short height, bool is_playboard = false)
        {
            IsPlayboard = is_playboard;
            this.width = width;
            this.height = height;
            count = (short)(width*height);
            grid = new BoardCard[count];
        }

        public void AddBoardCard(Card new_card)
        {
            int i = 0;
            while (i < count && grid[i] != null) i++;
            if (new_card != null && i != count) {
                SetGridCard(i, new BoardCard(new_card, i));
            }
        }
        public void SetBoardCard(Card new_card, int i, char force='p')
        {
            SetGridCard(i, (new_card != null) ? new BoardCard(new_card, i, force) : null);
        }
        public void SetBoardCard(BoardCard new_card, int i)
        {
            SetGridCard(i, new_card);
        }
        public BoardCard RemBoardCard(int i)
        {
            if (i >= count || grid[i] == null) return null;
            BoardCard t = grid[i];
            t.LocalCardChanged -= OnBoardCardChanged;
            t.CardMoved -= OnBoardCardMoved;
            BoardChanged -= t.SetAV;
            SetGridCard(i, null);
            t.board_i = -1;
            return t;
        }

        public void OnBoardCardChanged(BoardCard sender)
        {
            if (sender.HP <= 0 && IsPlayboard)
                RemBoardCard(sender.board_i);
        }
        public void OnBoardCardMoved(BoardCard sender, int prev_i)
        {
            SetGridCard(sender.board_i, sender);
            SetGridCard(prev_i, null);
        }

        public short GetPos(int i, int dx, int dy)
        {
            dx += (short)(i % width); // индексовые координаты
            dy += (short)(i / width);
            if (dx < 0 || dx >= width || dy < 0 || dy >= height) return -1;
            return (short)(dx + dy * width);
        }
        public BoardCard GetCardByPos(int i, int dx, int dy)
        {
            dx += (short)(i % width); // индексовые координаты
            dy += (short)(i / width);
            if (dx < 0 || dx >= width || dy < 0 || dy >= height) return null;
            return grid[dx + dy * width];
        }
        public short GetRDist(int i1, int i2) =>
            (short)Math.Max(
                Math.Abs(i1 % width - i2 % width),
                Math.Abs(i1 - i2) / width
            );
    }
}
