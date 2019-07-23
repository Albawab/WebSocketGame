namespace HenE.WebSocketExample.WebSocketClientApp
{
    using System;
    using HenE.WebSocketExample.WebSocketClient;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Local Client");

            Client server = new Client("localhost", 5000);
            server.Start();
            Console.WriteLine("Geef je naam op");

            // RdH: voor nu uit ivm debuggen
            // string naam = Console.ReadLine();
            // RdH: onderstaande 2 WEG als het spel een beetje loopt
            string naam = "A";
            Console.WriteLine(naam);

            server.VerzoekOmStartenSpel(naam, 3);

            /*
            ConsoleKeyInfo ingegevenTeken;
            Console.WriteLine("Welke teken wil je spelen,  O of X? ");
            ingegevenTeken = Console.ReadKey();

            string naamVanDeSpeler = string.Empty;
            do
            {
                if (ingegevenTeken.Key == ConsoleKey.O || ingegevenTeken.Key == ConsoleKey.X)
                {
                    switch (ingegevenTeken.Key)
                    {
                        case ConsoleKey.O:
                            server.VerzoekOmStartenSpel(naam, 3);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Je keuze is niet correct .Welke teken wil je spelen,  O of X? ");
                    ingegevenTeken = Console.ReadKey();
                }
            }
            while (ingegevenTeken.Key != ConsoleKey.O && ingegevenTeken.Key != ConsoleKey.X);

            */

            // Console.ReadKey();
            Console.WriteLine("I am done");
            Console.ReadKey();

            // client.Stop();
        }
    }
}
