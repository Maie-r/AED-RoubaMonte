using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace RoubaMonte
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string date = DateTime.Now.ToString().Replace(':', '-').Replace('/', '_');
            LogWriter log = new LogWriter($"matchlog{date}.txt");
            Console.WriteLine("Bem vindo ao Rouba Monte!\n");
            List<Jogador> TotalJogadores = new List<Jogador>();
            bool gameon = true;
            for (int o = 1; gameon; o++)
            {
                log.WriteLine($"Partida {o}");
                Console.Write("Quantos Jogadores: ");
                int playeramount = int.Parse(Console.ReadLine());
                Console.Write("Quantos Baralhos em jogo: ");
                int deckamount = int.Parse(Console.ReadLine());
                log.OnlyWriteLine($"Jogadores: {playeramount}\nBaralhos em jogo: {deckamount} ({deckamount * 52} cartas)");
                BuyDeck buydeck = new BuyDeck(deckamount);
                buydeck.Fill();
                buydeck.Shuffle();
                
                List<Jogador> players = new List<Jogador>();
                for (int i = 0; i < playeramount; i++)
                {
                    log.Write($"Nome do jogador {i + 1}: ");
                    string temp = Console.ReadLine();
                    log.OnlyWriteLine(temp);
                    int pos = ExistsIn(TotalJogadores, temp.ToLower());
                    if (pos >= 0)
                        players.Add(TotalJogadores[pos]);
                    else
                    {
                        Jogador tmp = new Jogador(temp);
                        players.Add(tmp);
                        TotalJogadores.Add(tmp);
                    }
                }
                Console.WriteLine("Modo rápido? (S/N)");
                string h = Console.ReadLine().ToLower();
                if (h == "s")
                {
                    GameProper(players, buydeck, log, true, false);
                }
                else if (h == "auto")
                {
                    GameProper(players, buydeck, log, true, true);
                }
                else
                {
                    GameProper(players, buydeck, log, false, false);
                }
                /*log.WriteLine("\n\nHistórico de pontuações:\n");
                foreach (Jogador player in TotalJogadores)
                {
                    player.DisplayRankings(log);
                }*/
                int menu;
                bool valid = true;
                log.WriteLine("1- Jogar outra rodada\n2- Mostrar ranking de um jogador\n3- Sair");
                while (valid)
                {
                    menu = int.Parse(Console.ReadLine());
                    switch (menu)
                    {
                        case 1:
                            log.OnlyWriteLine($"{menu}");
                            valid = false;
                            break;
                        case 2:
                            log.OnlyWriteLine($"{menu}");
                            log.WriteLine("Qual jogador?");
                            string player = Console.ReadLine().ToLower();
                            log.OnlyWriteLine(player);
                            int i = ExistsIn(TotalJogadores, player);
                            if (i < 0)
                            {
                                log.WriteLine("Não achamos este jogador.");
                            }
                            else
                            {
                                TotalJogadores[i].DisplayRankings(log);
                            }
                            log.WriteLine("\n1- Jogar outra rodada\n2- Mostrar ranking de um jogador\n3- Sair");
                            break;
                        case 3:
                            gameon = false;
                            valid = false;
                            break;
                    }
                }
                Console.Clear();
            }
            
        }

        static Jogador GameProper(List<Jogador> players, BuyDeck buydeck, LogWriter log, bool skip, bool auto)
        {
            /*Dictionary<int, Deck> PlayerDecks = new Dictionary<int, Deck>();
            for (int i = 0; i < players.Count; i++)
            {
                PlayerDecks.Add(players[i].ID, players[i].Monte);
            }*/
            List<Card> OnBoard = new List<Card>();
            //Jogador world = new Jogador("Mundo");
            int round = 0;
            while (buydeck.last > 0)
            {
                for (int i = 0; i < players.Count && buydeck.last > 0; i++) // players ordered clockwise
                {
                    round++;
                    log.WriteLine($"{round}");
                    Console.Clear();
                    log.OnlyWriteLine("");
                    bool lesserskip = true;
                    log.Write($"{players[i].Nome} pega uma carta do monte de compra: ");
                    Card cartahora = buydeck.Draw();
                    cartahora.Show(log);
                    log.WriteLine("");
                    List<int> available = ShowAvailable(OnBoard, players, cartahora, log);
                    if (available.Count == 0)
                    {
                        if (players[i].Monte.Count <= 0)
                        {
                            log.WriteLine($"Nenhum dos montes é roubavel, e o jogador não têm um monte, logo, {players[i].Nome} inicia um novo monte!");
                            players[i].Monte.Pile(cartahora);
                            //PlayerDecks.Add(players[i].ID, players[i].Monte);
                            // Create player deck
                        }
                        else
                        {
                            log.WriteLine("Nenhum dos montes é roubavel, e não encaixa no monte do jogador, logo, uma nova carta aparece na area de descarte!");
                            OnBoard.Add(cartahora);
                            // put card on board
                        }
                    }
                    else if (cartahora.Value == players[i].Monte.Peek().Value)
                    {
                        log.WriteLine("A carta encaixa em seu próprio monte, logo, é empilhado.");
                        players[i].Monte.Pile(cartahora);
                        // put card on own deck
                    }
                    else
                    {
                        log.WriteLine("Um monte pode ser roubado! Escolha: ");
                        int choice;
                        if (auto)
                        {
                            choice = 0;
                            while (!available.Contains(choice - 1))
                            {
                                choice++;
                            }
                        }
                        else
                        {
                            choice = -1;
                            while (!available.Contains(choice - 1))
                            {
                                choice = int.Parse(Console.ReadLine());
                            }
                        }
                        if (choice <= players.Count)
                        {
                            players[i].Roubar(players[choice - 1].Monte);
                        }
                        else
                        {
                            players[i].Roubar(OnBoard[choice - 1 - players.Count]);
                            OnBoard.RemoveAt(choice - 1 - players.Count);
                        }
                        lesserskip = false;
                        // get player deck
                    }
                    if (!skip)
                    {
                        if (lesserskip)
                            Console.ReadLine();
                    }
                }
            }
            Console.Clear();
            Console.WriteLine(round + " Rodadas");
            Dictionary<int, List<Jogador>> Winners = SetScore(players);
            log.WriteLine("Fim de Jogo!");
            log.Write("Vitória de ");
            string add = "";
            foreach (Jogador player in Winners[1])
            {
                log.Write(add + $"{player.Nome}");
                add = " e ";
            }
            log.WriteLine("\nRankings:");
            foreach (KeyValuePair<int, List<Jogador>> kv in Winners)
            {
                foreach (Jogador player in kv.Value)
                {
                    log.WriteLine($"#{player.Posição} - {player.Nome} - {player.monteAmount} Cartas");
                    player.Monte.Empty();
                }
            }
            return players[0];
        }

        static List<int> ShowAvailable(List<Card> OnBoard, List<Jogador> players, Card cartahora, LogWriter log)
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Monte.Count > 0)
                {
                    log.Write($"({players[i].Monte.Count}) {players[i].Nome}: ");
                    players[i].Monte.Peek().Show(log);
                    if (players[i].Monte.Peek().Value == cartahora.Value)
                    {
                        temp.Add(i);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        log.Write($" ({i + 1})");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    log.WriteLine("");
                }
                else
                {
                    log.Write($"({players[i].Monte.Count}) {players[i].Nome}: VAZIO\n");
                }
            }
            log.WriteLine("Area de descarte:");
            for (int i = 0; i < OnBoard.Count; i++)
            {
                OnBoard[i].Show(log);
                if (temp.Count <= 0)
                {
                    if (OnBoard[i].Value == cartahora.Value)
                    {
                        temp.Add(i + players.Count);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        log.Write($" ({i + players.Count + 1})");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                log.WriteLine("");
            } 
            return temp;
        }

        static Dictionary<int, List<Jogador>> SetScore(List<Jogador> players)
        {
            Dictionary<int, List<Jogador>> Winners = new Dictionary<int, List<Jogador>>();
            players[0].monteAmount = players[0].Monte.Count;
            for (int i = 1; i < players.Count; i++)
            {
                players[i].monteAmount = players[i].Monte.Count;
                Jogador current = players[i];
                int j = i - 1;
                while (j >= 0 && players[j].monteAmount > current.monteAmount)
                {
                    players[j + 1] = players[j];
                    j--;
                }
                players[j + 1] = current;
            }
            players.Reverse();
            Winners.Add(1, new List<Jogador>());
            Winners[1].Add(players[0]);
            players[0].Posição = 1;
            if (players[0].Ranks.Count >= 5)
            {
                players[0].Ranks.Dequeue();
            }
            players[0].Ranks.Enqueue(1);
            for (int i = 1, j = 1; i < players.Count; i++)
            {
                if (players[i-1].monteAmount > players[i].monteAmount)
                {
                    j++;
                }
                if (players[i].Ranks.Count >= 5)
                {
                    players[i].Ranks.Dequeue();
                }
                players[i].Ranks.Enqueue(j);
                if (Winners.ContainsKey(j))
                {
                    Winners[j].Add(players[i]);
                    players[i].Posição = j;
                }
                else
                {
                    Winners.Add(j, new List<Jogador>());
                    Winners[j].Add(players[i]);
                    players[i].Posição = j;
                }
            }
            return Winners;
        }

        static int ExistsIn(List<Jogador> players, string name)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].Nome.ToLower() == name)
                    return i;
            }
            return -1;
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
