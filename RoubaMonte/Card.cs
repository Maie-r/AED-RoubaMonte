using System;
using System.Collections.Generic;
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
}
