namespace HenE.WebSocketExample.WebSocketServerApp
{
    using HenE.WebSocketExample.WebSocketServer;
    using System;
    using System.Net;

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting server");

            IPAddress iPAddress;
            int poort;
            // vraag het ipadres uit en de poort
            //Console.ReadLine
            Console.WriteLine("Mag ik weten welke IPAddress en de Poort die je wil gebruiken ?");
            Console.WriteLine("voeg hier je IPAdress :");
            iPAddress = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("voeg hier je poort :");
            poort = int.Parse(Console.ReadLine());

            // 10.0.0.184
            Server srv = new Server(iPAddress, poort);

            srv.Start();

            Console.ReadLine();
        }
    }
}
