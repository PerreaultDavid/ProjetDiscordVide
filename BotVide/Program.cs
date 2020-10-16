using System;
using System.Threading;

namespace BotVide
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Bot Sans Nom";
            Console.WriteLine("Ouverture du {0} (MongoDB)", Console.Title);

            var bot = new Bot();
            Console.WindowHeight = 2;
            Console.WindowWidth = 100;
            Thread.Sleep(1000);
            bot.RunAsync().GetAwaiter().GetResult();

            Console.WriteLine("{0} Fermé!", Console.Title);
        }
    }
}
