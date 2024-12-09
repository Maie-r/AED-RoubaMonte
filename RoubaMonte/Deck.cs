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
    }
}
