using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

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
        public int res;
        public Player(string name)
        {
            this.name = name;
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
                Globals.Table += bet * 2;
                Console.WriteLine($"{name} your bank: {bank}$, your bet {Bet}$");
            }
        }
        public List<int> Sum {  get { return sum; } set { sum = value; } }
        public override void TakeCard()
        {
            var cardAt = Globals.random.Next(Globals.deck.Count);
            var card = Globals.deck[cardAt];
            hand.Add(card);
            Globals.deck.RemoveAt(cardAt);
            Console.WriteLine($"{name} took a card: {card} ");
            Thread.Sleep(1500);
            CountSum(card);
        }
        public override void Win()
        {
            Console.WriteLine($"{name} wins {Globals.Table}!");
            bank += Globals.Table;
            bet = 0;
            Globals.Table = 0;
            hand.RemoveAll(item => item is Card);
            sum = new List<int> { 0, 0 };
        }
        public override void Loose()
        {
            Console.WriteLine($"{name} looses {Bet}!");
            bet = 0;
            Globals.Table = 0;
            hand.RemoveAll(item => item is Card);
            sum = new List<int> { 0, 0 };
        }
        public void Draw()
        {
            Console.WriteLine($"Draw!");
            bank += Globals.Table / 2;
            bet = 0;
            Globals.Table = 0;
            hand.RemoveAll(item => item is Card);
            sum = new List<int> { 0, 0 };

        }
        public void DealerRes()
        {
            hand.RemoveAll(item => item is Card);
            sum = new List<int> { 0, 0 };
        }
        public void CountSum(Card card)
        {
            Predicate <Card> predicate = x => x.rank == 1;
            var handAr = hand.ToArray();
            bool hasAce = hand.Contains(Array.Find(handAr, predicate));
            sum[0] += card.rank;
            if (card.rank == 1 && sum[1] + 11 <= 21) 
            { 
                sum[1] += 11; 
            }
            else 
            { 
                sum[1] += card.rank; 
            }
            if (sum[1] == 21)
            {
                Console.WriteLine($"sum of {name}: {sum[1]}");
            }
            else if (sum[1] <= 21 && sum[0] != sum[1]) 
            { 
                Console.WriteLine($"sum of {name}: {sum[0]} or {sum[1]}"); 
            }
            else 
            { 
                Console.WriteLine($"sum of {name}: {sum[0]}"); 
            }
        }
        public void PlayerStep()
        {
            Console.WriteLine("insert your bet:");
            Bet = Convert.ToInt32(Console.ReadLine());
            TakeCard();
            TakeCard();
            if (sum[1] == 21)
            {
                Console.WriteLine("BlackJack!");
            }
            string status;
            while (sum[0] < 21 && (sum[1] < 21 || sum[1] > 21))
            {
                Console.WriteLine("Take another card? y/n");
                status = Console.ReadLine();
                if (status == "y")
                {
                    TakeCard();
                }
                else if (status == "n") 
                {
                    break;
                }
                else 
                { 
                    Console.WriteLine("invalid data"); 
                    continue; 
                }
            }
            var query = from i in sum
                        where (i == 21) || ((i == sum.Max() && sum[0] < 21 && sum[1] < 21)) || (i == sum.Min() && sum[1] > 21)
                        select i;
            res = query.ElementAt(0);
        }
        public void DealerStep()
        {
            TakeCard();
            TakeCard();
            if (sum[1] == 21)
            {
                Console.WriteLine("BlackJack!");
            }
            while (sum[1] < 17 && sum[0] < 17 || sum[0] < 17 && sum[1] > 21) 
            { 
                TakeCard();
            }
            var query = from i in sum
                        where (i == 21) || ((i == sum.Max() && sum[0] < 21 && sum[1] < 21)) || (i == sum.Min() && sum[1] > 21)
                        select i;
            res = query.ElementAt(0);
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
            get { return name; }
            set { name = $"{value} of {suit}"; }
        }
        public override string ToString()
        {
            return name;
        }
     }
    public static class Globals
    {
        public static List<Card> deck;
        public static List<Card> deckCopy;
        public static Random random;
        private static int table = 0;
        public static int Table
        {
            get { return table; }
            set 
            { 
                table = value;
                Console.WriteLine($"{value} is on table");
            }
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
        public static void Compare(Player player, Player dealer)
        {
            var pr = player.res;
            var dr = dealer.res;
            Console.WriteLine($"{player.name} result: {pr}, Dealer result: {dr}");
            if (pr == dr)
            {
                player.Draw();
                dealer.DealerRes();
            }
            else if ((pr > dr && pr <= 21) || (pr < dr && dr > 21))
            {
                player.Win();
                dealer.DealerRes();
            }
            else if ((dr > pr && dr <= 21) || (dr < pr && pr > 21))
            {
                player.Loose();
                dealer.DealerRes();
            }
        }
        public static void Main(string[] args)
        {
            Globals.deck = new List<Card>();
            for (int i = 0; i < 4; i++)
            {
                var deck = GenerateDeck();
                Globals.deck.AddRange(deck);
            }
            Globals.deckCopy = Globals.deck;
            Globals.random = new Random();
            Console.WriteLine("insert your name");
            var player = new Player(Console.ReadLine());
            var dealer = new Player("Dealer");
            var index = true;
            while (index)
            {
                player.PlayerStep();
                dealer.DealerStep();
                Compare(player, dealer);
                Globals.deck = Globals.deckCopy;
                Console.WriteLine("Continue ? y/n");
                if (Console.ReadLine() == "n") { break; }
                else { continue; }

            }
        }
    }
}