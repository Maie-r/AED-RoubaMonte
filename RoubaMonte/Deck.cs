using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoubaMonte
{
    internal class Deck
    {
        public Card Bottom;
        public Card Top;
        public Jogador Owner;
        int count;

        public Deck()
        {
            Top = Bottom = null;
            Count = 0;
        }
        public Deck(Jogador player)
        {
            Owner = player;
            Top = Bottom = null;
            Count = 0;
        }

        public Deck(Card carta, Jogador player)
        {
            Owner = player;
            Top = Bottom = carta;
            Count = 1;
        }

        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        public void Pile(Card card)
        {
            if (Bottom == null)
            {
                Bottom = card;
            }
            card.Next = Top;
            Top = card;
            Count++;
        }

        public Card Draw()
        {
            Card temp = Top;
            Top = Top.Next;
            Count--;
            return temp;
        }
        public Card Peek()
        {
            if (Top == null)
            {
                return new Card(-1, ' ');
            }
            return Top;
        }

        public void Empty()
        {
            Top = Bottom = null;
            Count = 0;
        }

        /*public void Fill()
        {
            char type = '♥';
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 13; j++)
                {
                    Card t = null;
                    if (j >= 10)
                    {
                        switch (j)
                        {
                            case 10:
                                t = new Card("J", type);
                                break;
                            case 11:
                                t = new Card("Q", type);
                                break;
                            case 12:
                                t = new Card("K", type);
                                break;
                                
                        }
                    }
                    else if (j == 0)
                    {
                        t = new Card("A", type);
                    }
                    else
                    {
                        t = new Card($"{j + 1}", type);
                    }
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
        }*/
    }
}
