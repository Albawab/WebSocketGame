// <copyright file="Program.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketClientApp
{
    using System;
    using System.Net;
    using HenE.WebSocketExample.WebSocketClient;

    /// <summary>
    /// De progran van de client.
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Local Client");
            try
            {
                IPAddress iPAddress;
                int poort;
                Console.WriteLine("Mag ik weten welke IPAddress en de Poort die je wil gebruiken ?");
                Console.WriteLine("voeg hier je IPAdress :");
                iPAddress = IPAddress.Parse(Console.ReadLine());
                Console.WriteLine("voeg hier je poort :");
                poort = int.Parse(Console.ReadLine());

                Client server = new Client(iPAddress, poort);
                server.Start();
                Console.WriteLine("Hoi, Leuk dat je komt spelen, wil je me je naam vertellen?");

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

                Console.WriteLine("Wil je misschien tegen de computer speler , dus je hoeft niet lang te wachte , J of N ?");
                string vraagTegenComputer = Console.ReadLine().ToLowerInvariant();
                while ((vraagTegenComputer != "j") && (vraagTegenComputer != "n"))
                {
                    Console.WriteLine("Je mag alleen J of N invoeren .");
                    Console.WriteLine("J of N ??");
                    vraagTegenComputer = Console.ReadLine().ToLowerInvariant();
                }

                if (vraagTegenComputer == "j")
                {
                    server.SpelTegenComputer(naam, dimensionHetBord);
                }
                else
                {
                    server.VerzoekOmStartenSpel(naam, dimensionHetBord);
                }

                Console.WriteLine("I am done");
                Console.ReadKey();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                Console.WriteLine("--");
                Console.WriteLine(exp.ToString());
                Console.ReadLine();
            }

            // client.Stop();
        }
    }
}
