using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoubaMonte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string date = DateTime.Now.ToString().Replace(':', '-').Replace('/', '_');
            LogWriter log = new LogWriter($"matchlog{date}.txt");
            Console.WriteLine("Bem vindo ao Rouba Monte!\n");
            bool gameon = true;
            for (int o = 1; gameon; o++)
            {
                log.WriteLine($"Partida {o}");
                Console.Write("Quantos Jogadores: ");
                int playeramount = int.Parse(Console.ReadLine());
                Console.Write("Quantos Baralhos em jogo: ");
                int deckamount = int.Parse(Console.ReadLine());
                log.OnlyWriteLine($"Jogadores: {playeramount}\nBaralhos em jogo: {deckamount}");
                BuyDeck buydeck = new BuyDeck(deckamount);
                buydeck.Fill();
                buydeck.Shuffle();
                /*for (int i = 0; i < 52*buydeck.multiplier; i++)
                {
                    buydeck.Draw().Show();
                    Console.WriteLine();
                }*/
                List<Jogador> players = new List<Jogador>();
                for (int i = 0; i < playeramount; i++)
                {
                    log.Write($"Nome do jogador {i + 1}: ");
                    string temp = Console.ReadLine();
                    log.OnlyWriteLine(temp);
                    players.Add(new Jogador(i, temp));
                }
                Jogador Winner = GameProper(players, buydeck, log);
                Console.WriteLine("Jogar outra rodada? (S/N)");
                if (Console.ReadLine().ToLower() == "s")
                {
                    Console.WriteLine(""); // afterwards will need to check for same players and different ones
                }
                else
                {
                    gameon = false;
                }
                Console.Clear();
            }
            
        }

        static Jogador GameProper(List<Jogador> players, BuyDeck buydeck, LogWriter log)
        {
            List<Deck> available = new List<Deck>();
            while (buydeck.last > 0)
            {
                for (int i = 0; i < players.Count;i++) // players ordered clockwise
                {
                    Console.Clear();
                    log.Write($"{players[i].Nome} pega uma carta do monte de compra: ");
                    Card cartahora = buydeck.Draw();
                    cartahora.Show(log);
                    log.WriteLine("");
                    if (ShowAvailable(available, log))
                    {
                        // check for available to steal
                    }
                    else
                    {
                        // put card on board
                    }
                    Console.ReadLine();
                }
            }
            return players[0];
        }

        static bool ShowAvailable(List<Deck> available, LogWriter log)
        {
            if (available.Count <= 0)
                return false;
            foreach (Deck deck in available)
            {
                log.WriteLine($"{deck.Owner}: {deck.Peek()}");
            }
            return true;
        }
    }
    class LogWriter
    {
        string path;
        StreamWriter writer;

        public LogWriter(string path)
        {
            this.path = path;
            writer = new StreamWriter(path, false);
            writer.Write("");
            writer.Close();
        }

        public void WriteLine(string text)
        {
            writer = new StreamWriter(path, true);
            writer.WriteLine(text);
            Console.WriteLine(text);
            writer.Close();
        }

        public void Write(string text)
        {
            writer = new StreamWriter(path, true);
            writer.Write(text);
            Console.Write(text);
            writer.Close();
        }
        public void OnlyWriteLine(string text)
        {
            writer = new StreamWriter(path, true);
            writer.WriteLine(text);
            writer.Close();
        }
        public void OnlyWrite(string text)
        {
            writer = new StreamWriter(path, true);
            writer.Write(text);
            writer.Close();
        }

    }
}
