using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack_cSharp 
{
    public abstract class AbstractPlayer
    {
        public abstract int Bet{ get; set;}
        public abstract void TakeCard();
        public abstract void Win();
        public abstract void Loose();
    }
    public class Player : AbstractPlayer
    {
        public List<Card> hand = new List<Card>();
        public string name;
        public int bank;
        private int bet;
        private List<int> sum;
        public Player()
        {
            Console.WriteLine("insert your name");
            name = Console.ReadLine();
            bank = 1000;
            sum = new List<int> { 0, 0 };
        }
        public override int Bet
        {
            get { return bet; }
            set 
            { 
                if (value > bank)
                {
                    while (value > bank)
                    {
                        Console.WriteLine("your bet is bigger than your bank, please, try again");
                        value = Convert.ToInt32(Console.ReadLine());
                    }
                }
                bet = value;
                bank -= bet;
                Globals.table += bet;
            }
        }
        public List<int> Sum {  get { return sum; } set { sum = value; } }
        public override void TakeCard()
        {
            var cardAt = Globals.random.Next(Globals.deck.Count);
            var card = Globals.deck[cardAt];
            hand.Add(card);
            CountSum(card);
            Globals.deck.RemoveAt(cardAt);
            Console.WriteLine($"{name} took a card: {card} ");
        }
        public override void Win()
        {
            bank += Globals.table;
            bet = 0;
            Globals.table = 0;
            hand.RemoveAll(item => item is Card);
            Console.WriteLine($"{name} win!");
        }
        public override void Loose()
        {
            bet = 0;
            hand.RemoveAll(item => item is Card);
            Console.WriteLine($"{name} loose!");
        }
        public void CountSum(Card card)
        {
            sum[0] += card.rank;
            if ((card.rank == 1) && (sum[0] + 11 < 21)) 
            { 
                sum[1] += 11;
                Console.WriteLine($"sum of {name}: {sum[0]} or {sum[1]}");
            }
            else 
            { 
                sum[1] += card.rank;
                Console.WriteLine($"sum of {name}: {sum[0]}");
            }
        }
    }
    public class Dealer : Player
    {
        public List<Card> hand = new List<Card>();
        public string name;
        public double bank = double.PositiveInfinity;
        private int bet;

        public Dealer()
        {
            name = "Dealer";
        }

    }
    public class Card
    {
        public dynamic rank;
        public string suit;
        private string name;
        public Card(dynamic rank, string suit)
        {
            this.suit = suit;
            Name = rank;
            if (rank is char)
            {
                if (rank == 'A') { this.rank =  1; }
                else { this.rank = 10; }
            }
            else { this.rank = rank; }
        }
        public dynamic Name
        {
            get { return this.name; }
            set { this.name = $"{value} of {this.suit}"; }
        }
        public override string ToString()
        {
            return this.name;
        }
     }
    public static class Globals
    {
        public static List<Card> deck;
        public static Random random;
        public static int table;
    }
    public class Program
    {
        public static List<Card> GenerateDeck()
        {
            var values = new List<dynamic>() { 2, 3, 4, 5, 6, 7, 8, 9, 10, 'J', 'Q', 'K', 'A' };
            var suits = new List<string>() { "clubs", "hearts", "spades", "diamonds" };
            var deck = new List<Card>();
            foreach (var i in suits)
            {
                foreach (var j in values)
                {
                    deck.Add(new Card(j, i));
                }
            }
            return deck;
        }
        public static void Compare(Player player, Dealer dealer)
        {
            

        }
        public static void Main(string[] args)
        {   
            Globals.deck = GenerateDeck();
            Globals.random = new Random();
            var player = new Player();
            int i = 0;
            while (Globals.deck.Count != 0)
            {
                player.TakeCard();
                i++;
                Console.WriteLine(i);
            }
        }
    }
}