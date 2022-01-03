using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack_cSharp 
{
    abstract class AbstractPlayer
    {
        public abstract int Bet{ get; set;}
        public abstract void TakeCard();
        public abstract void Stand();
        public abstract void Win();
        public abstract void Loose();
    }
    public class Player : AbstractPlayer
    {
        public Card[] hand;
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
                    bet = value;
                }
                else { bet = value; }
            }
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
            var deck = GenerateDeck();
        }
    }
}