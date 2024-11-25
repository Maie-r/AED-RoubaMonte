using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoubaMonte
{
    internal class Jogador
    {

        public string Nome;
        public int ID;
        public int Posição;
        public int monteAmount;
        Deck monte;
        Queue<int> ranks;
        public Jogador(int id, string nome)
        {
            Nome = nome;
            ID = id;
            monteAmount = 0;
            Posição = 0;
            monte = new Deck(this);
            ranks = new Queue<int>();
        }
        public Deck Monte
        {
            get { return monte; }
            set { monte = value; }
        }
        public Queue<int> Ranks
        {
            get { return ranks; }
            set { ranks = value; }
        }

        public void Roubar(Deck monteroubado) // make check in the game
        {
            monteroubado.Bottom.Next = monte.Top;
            monte.Top.Next = monteroubado.Top.Next;
            monte.Top = monteroubado.Top;
            monteroubado.Top = null;
            monteroubado = null;
        }
    }
}
