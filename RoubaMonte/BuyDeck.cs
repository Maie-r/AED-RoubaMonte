using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoubaMonte
{
    internal class BuyDeck
    {
        Card[] deck;
        public int last;
        public int multiplier;

        public BuyDeck(int n)
        {
            deck = new Card[52*n];
            multiplier = n;
            last = 0;
        }

        public void Pile(Card card)
        {
            deck[last] = card;
            last++;
        }

        public Card Draw()
        {
            return deck[--last];
        }

        public void Fill()
        {
            for (int k = 0; k < multiplier; k++)
            {
                char type = '♥';
                for (int i = 0; i < 4; i++)
                {
                    for (int j = 1; j < 14; j++)
                    {
                        Card t = new Card(j, type);
                        Pile(t);
                    }
                    switch (i)
                    {
                        case 0:
                            type = '♦';
                            break;
                        case 1:
                            type = '♣';
                            break;
                        case 2:
                            type = '♠';
                            break;
                    }
                }
            }
        }
        public void Shuffle()
        {
            Random r = new Random();
            for (int n = deck.Length - 1; n > 0; --n)
            {
                int t = r.Next(n + 1);
                Card temp = deck[n];
                deck[n] = deck[t];
                deck[t] = temp;
            }
        }
    }
}
