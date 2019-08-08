// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketServerApp
{
    using System;
    using System.Net;
    using HenE.WebSocketExample.WebSocketServer;

    /// <summary>
    /// De program van de server.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Starting server");

            IPAddress iPAddress;
            int poort;
            Console.WriteLine("Mag ik weten welke IPAddress en de Poort die je wil gebruiken ?");
            Console.WriteLine("voeg hier je IPAdress :");
            iPAddress = IPAddress.Parse(Console.ReadLine());
            Console.WriteLine("voeg hier je poort :");
            poort = int.Parse(Console.ReadLine());

            Server srv = new Server(iPAddress, poort);

            srv.Start();

            Console.ReadLine();
        }
    }
}
