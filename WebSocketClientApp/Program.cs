namespace HenE.WebSocketExample.WebSocketClientApp
{
    using HenE.WebSocketExample.WebSocketClient;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Client Een");

            Client client = new Client("localhost", 5000);

            Console.WriteLine("Geef je naam op");
            string naam = Console.ReadLine();

            client.Start();

            client.VerzoekOmStartenSpel(naam, 3);

            Console.WriteLine("I am ready");
            Console.ReadKey();

            //  client.Stop();
        }
    }
}
