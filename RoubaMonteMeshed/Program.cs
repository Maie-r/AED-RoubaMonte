using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoubaMonte
{
    internal class Card
    {
        int value;
        char type;
        Card next;

        public Card(int value, char type)
        {
            this.value = value;
            this.type = type;
        }
        public Card Next
        {
            get { return next; }
            set { next = value; }
        }

        public int Value
        {
            get { return value; }
        }
        public void Show()
        {
            Console.BackgroundColor = ConsoleColor.White;
            if (type == '♥' || type == '♦')
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            string value = "";
            if (this.value > 10)
            {
                switch (this.value)
                {
                    case 11:
                        value = "J";
                        break;
                    case 12:
                        value = "Q";
                        break;
                    case 13:
                        value = "K";
                        break;

                }
            }
            else if (this.value == 1)
            {
                value = "A";
            }
            else
            {
                value = $"{this.value}";
            }
            Console.Write($" {type} {value} {type} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
        public void Show(LogWriter log)
        {
            Console.BackgroundColor = ConsoleColor.White;
            if (type == '♥' || type == '♦')
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            string value = "";
            if (this.value > 10)
            {
                switch (this.value)
                {
                    case 11:
                        value = "J";
                        break;
                    case 12:
                        value = "Q";
                        break;
                    case 13:
                        value = "K";
                        break;

                }
            }
            else if (this.value == 1)
            {
                value = "A";
            }
            else
            {
                value = $"{this.value}";
            }
            log.Write($" {type} {value} {type} ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
    internal class BuyDeck
    {
        Card[] deck;
        public int last;
        public int multiplier;

        public BuyDeck(int n)
        {
            deck = new Card[52 * n];
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
            List<Card> OnBoard = new List<Card>();
            while (buydeck.last > 0)
            {
                for (int i = 0; i < players.Count && buydeck.last > 0; i++)
                {
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
                        }
                        else
                        {
                            log.WriteLine("Nenhum dos montes é roubavel, e não encaixa no monte do jogador, logo, uma nova carta aparece na area de descarte!");
                            OnBoard.Add(cartahora);
                        }
                    }
                    else if (cartahora.Value == players[i].Monte.Peek().Value)
                    {
                        log.WriteLine("A carta encaixa em seu próprio monte, logo, é empilhado.");
                        players[i].Monte.Pile(cartahora);
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
                            players[i].Monte.Pile(cartahora);
                        }
                        else
                        {
                            players[i].Roubar(OnBoard[choice - 1 - players.Count]);
                            players[i].Monte.Pile(cartahora);
                            OnBoard.RemoveAt(choice - 1 - players.Count);
                        }
                        lesserskip = false;
                    }
                    if (!skip)
                    {
                        if (lesserskip)
                            Console.ReadLine();
                    }
                }
            }
            Console.Clear();
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
                if (players[i - 1].monteAmount > players[i].monteAmount)
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
