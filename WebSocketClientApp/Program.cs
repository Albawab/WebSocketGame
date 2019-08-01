// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketClientApp
{
    using System;
    using HenE.WebSocketExample.WebSocketClient;

    /// <summary>
    /// De progran van de client.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Local Client");

            Client server = new Client("localhost", 5000);
            server.Start();
            Console.WriteLine("Geef je naam op");

            string naam = Console.ReadLine();

            short dimensionHetBord = 0;

            do
            {
                Console.WriteLine("Wat is de dimension van het bord ? \" Geef een nummer tussen 2 en 9 \"");
                string readDimension = Console.ReadLine();
                if (short.TryParse(readDimension, out dimensionHetBord))
                {
                    dimensionHetBord = short.Parse(readDimension);
                }
                else
                {
                    Console.WriteLine("U hebt geen nummer ingevoerd");
                }
            }
            while (dimensionHetBord < 2 || dimensionHetBord > 9);

            server.VerzoekOmStartenSpel(naam, dimensionHetBord);

            Console.WriteLine("I am done");
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();
            Console.ReadKey();

            // client.Stop();
        }
    }
}
