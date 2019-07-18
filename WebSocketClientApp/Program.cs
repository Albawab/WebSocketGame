namespace HenE.WebSocketExample.WebSocketClientApp
{
    using HenE.Abdul.Game_OX;
    using HenE.Abdul.GameOX;
    using HenE.WebSocketExample.WebSocketClient;
    using HenE.WebSocketExample.WebSocketServer;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("Client Een");

            Client client = new Client("localhost", 5000);
            client.Start();
            Console.WriteLine("Geef je naam op");
            string naam = Console.ReadLine();

            ConsoleKeyInfo ingegevenTeken;
            Console.WriteLine("Welke teken wil je spelen,  O of X? ");
            ingegevenTeken = Console.ReadKey();


            string naamVanDeTweedeSpeler = string.Empty;
            do
            {
                if (ingegevenTeken.Key == ConsoleKey.O || ingegevenTeken.Key == ConsoleKey.X)
                {
                    switch (ingegevenTeken.Key)
                    {
                        case ConsoleKey.O:
                            client.VerzoekOmStartenSpel(naam, 3);;
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




          

            Console.ReadKey();
            Console.WriteLine("I am ready");
            Console.ReadKey();

            //  client.Stop();
        }
    }
}
