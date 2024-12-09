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
        public int Posição;
        public int monteAmount;
        Deck monte;
        Queue<int> ranks;
        public Jogador(string nome)
        {
            Nome = nome;
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

        public void Roubar(Deck monteroubado)
        {
            monte.Count += monteroubado.Count;
            monteroubado.Bottom.Next = monte.Top;
            monte.Top = monteroubado.Top;
            if (monte.Bottom == null)
            {
                monte.Bottom = monte.Top;
            }
            monte.Top.Next = monteroubado.Top.Next;
            monteroubado.Empty();
        }
        public void Roubar(Card cartaroubada)
        {
            monte.Count++;
            cartaroubada.Next = monte.Top;
            monte.Top = cartaroubada;
            if (monte.Bottom == null)
            {
                monte.Bottom = monte.Top;
            }
        }

        public void DisplayRankings(LogWriter log)
        {
            log.Write($"{Nome}: \t");
            string ad = "";
            foreach (int pos in Ranks)
            {
                log.Write($"{ad}{pos}");
                ad = "-";
            }
            log.WriteLine("");
        }
    }
}
