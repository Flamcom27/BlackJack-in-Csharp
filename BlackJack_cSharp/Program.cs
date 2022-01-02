using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackJack_cSharp 
{
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
        public static void Main(string[] args)
        {
            var a = new Card(2, "Clubs");
            var b = new Card('A', "Clubs");
            var c = new Card('K', "Clubs");
            foreach (var i in b.rank)
            {
                Console.WriteLine($" rank: {i}");
            }

        }
    }
}