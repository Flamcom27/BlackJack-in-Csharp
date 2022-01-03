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
        public int sum;
        private int bet;
        public Player()
        {
            Console.WriteLine("insert your name");
            name = Console.ReadLine();
            bank = 1000;
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
                Globals.table += bet * 2;
                Console.WriteLine($"your bet: {bet}, your bank: {bank}");
            }
        }
        public override void TakeCard()
        {
            var cardAt = Globals.random.Next(Globals.deck.Count);
            var card = Globals.deck[cardAt];
            hand.Add(card);
            Globals.deck.RemoveAt(cardAt);
            Console.Write("You have: ");
            foreach (Card i in hand)
            {
                Console.Write($"{i} ");
            }
        }
        public override void Win()
        {
            bank += Globals.table;
            bet = 0;
            Globals.table = 0;
            hand.RemoveAll(item => item is Card);
            Console.WriteLine($"You win! your bank: {bank}");
        }
        public override void Loose()
        {
            bet = 0;
            hand.RemoveAll(item => item is Card);
            Console.WriteLine($"You loose! your bank: {bank}");
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
                if (rank == 'A') { this.rank = new List<int>() { 1, 11 }; }
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
        public static Player player;
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
        public static void Main(string[] args)
        {   
            Globals.deck = GenerateDeck();
            Globals.random = new Random();
            Globals.player = new Player();
            while (true)
            {
                var i = Console.ReadLine();
                switch (i)
                {
                    case "1":
                        Console.WriteLine("insert your bet: ");
                        Globals.player.Bet = Convert.ToInt32(Console.ReadLine());
                        continue;
                    case "2":
                        Globals.player.TakeCard();
                        continue;
                    case "3":
                        Globals.player.Win();
                        continue;
                    case "4":
                        Globals.player.Loose();
                        continue;
                }
            }
        }
    }
}