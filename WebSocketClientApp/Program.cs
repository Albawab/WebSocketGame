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
            Console.ReadKey();



            client.Start();

            client.VerzoekOmStartenSpel("Jos", 3);

            Console.WriteLine("I am ready");
            Console.ReadKey();

            //  client.Stop();
        }
    }
}
