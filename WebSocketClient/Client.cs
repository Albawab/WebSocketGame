﻿// <copyright file="Client.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketClient
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    /// De class van de client.
    /// </summary>
    public class Client : WebsocketBase
    {
        private TcpClient tcpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="hostname">Host naam.</param>
        /// <param name="serverPort"> Server Port.</param>
        public Client(IPAddress hostname, int serverPort)
        {
            this.Hostname = hostname;
            this.ServerPort = serverPort;
        }

        /// <summary>
        /// Gets server Port.
        /// </summary>
        public int ServerPort { get; private set; }

        /// <summary>
        /// Gets host naam.
        /// </summary>
        public IPAddress Hostname { get; private set; }

        /// <summary>
        /// Het client start.
        /// </summary>
        public void Start()
        {
            this.tcpClient = new TcpClient(this.Hostname.ToString(), this.ServerPort);
        }

        /// <summary>
        /// The clinet start.
        /// </summary>
        public void Stop()
        {
            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
            }
        }

        /// <summary>
        /// Stuur de naam met de dimension naar de server.
        /// </summary>
        /// <param name="spelersnaam">Speler naam.</param>
        /// <param name="dimension">Dimension.</param>
        public void VerzoekOmStartenSpel(string spelersnaam, short dimension)
        {
            string message = CommandoHelper.CreateVerzoekTotDeelnemenSpelCommando(spelersnaam, dimension);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }

        /// <summary>
        /// Stuur een message naar de server dat de speler tegen de computer wil spelen.
        /// </summary>
        /// <param name="naam">De naam van de speler.</param>
        /// <param name="dimension">dimension.</param>
        public void SpelTegenComputer(string naam, short dimension)
        {
            string message = CommandoHelper.CreateSpelTegenComputer(naam, dimension);
            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }

        /// <inheritdoc/>
        protected override string ProcessStream(string stream, TcpClient client)
        {
            // bepaal het event
            // het event is het gedeelte in de msg voor de #
            // daarna komen de parameters
            string eventParams = string.Empty;
            string returnMessage = null;
            Events events = EventHelper.SplitEventAndParamsFromMessage(stream, out eventParams);

            string[] opgeknipt = eventParams.Split(new char[] { '&' });
            string[] opgekniptDerdeDeel = eventParams.Split(new char[] { ',' });

            try
            {
                switch (events)
                {
                    case Events.Error:
                        Console.WriteLine(eventParams);
                        break;

                    case Events.SpelGestart:
                        Console.WriteLine("We gaan starten " + opgeknipt[0] + " Tegen " + opgeknipt[1] + " De dimension is : " + opgeknipt[2]);
                        break;

                    case Events.SpelerGestart:
                        Console.WriteLine(opgeknipt[1]);
                        Console.WriteLine(" Je mag starten !!");
                        Console.WriteLine("Welke teken wil je gebruiken, O of X ?");
                        ConsoleKeyInfo teken;
                        teken = Console.ReadKey();
                        while (teken.Key != ConsoleKey.O && teken.Key != ConsoleKey.X)
                        {
                            Console.WriteLine("Fout ! Je mag andre teken gepruiken.");
                            teken = Console.ReadKey();
                        }

                        this.SpelerGestart(teken.Key.ToString());
                        break;

                    case Events.YourTurn:
                        if (opgeknipt.Length > 1)
                        {
                            Console.WriteLine(opgeknipt[1]);
                        }

                        Console.WriteLine();
                        Console.WriteLine("Geef een nummer toe !!");
                        string nummerString = Console.ReadLine();
                        short nummer;
                        while (!short.TryParse(nummerString, out nummer))
                        {
                            Console.WriteLine("Geef maar een nummer toe !!!");
                            nummerString = Console.ReadLine();
                        }

                        this.DoeZet(nummer);
                        break;

                    case Events.Winnaar:
                        string[] opgekniptDerdeDeelDrie = opgekniptDerdeDeel[0].Split(new char[] { '&' });
                        Console.WriteLine(opgekniptDerdeDeel[1] + " Is gewonnen !!!!");
                        Console.WriteLine(opgekniptDerdeDeel[1] + " heeft : " + opgekniptDerdeDeelDrie[1] + " Punt / Punten !");
                        break;

                    case Events.BordIsVol:
                        Console.WriteLine("Het bord is vol !!");
                        break;

                    case Events.NieuwRondje:
                        Console.WriteLine("Wil je nog een rondje , J of N?");
                        string nieuwRondjes = Console.ReadLine();
                        nieuwRondjes.ToLower();
                        while ((nieuwRondjes != "j") && (nieuwRondjes != "n"))
                        {
                            Console.WriteLine("Geef maar J of N !");
                            nieuwRondjes = Console.ReadLine();
                        }

                        switch (nieuwRondjes)
                        {
                            case "j":
                                this.NieuwRondje(nieuwRondjes);
                                break;
                            case "n":
                                this.BeeindigSpel();
                                break;
                        }

                        break;

                    case Events.StartNieuwRond:
                        Console.WriteLine(opgekniptDerdeDeel[0]);
                        Console.WriteLine("Geef een nummer toe !!");
                        string nummerString2 = Console.ReadLine();
                        short nNnummer;
                        while (!short.TryParse(nummerString2, out nNnummer))
                        {
                            Console.WriteLine("Geef maar een nummer toe !!!");
                            nummerString = Console.ReadLine();
                        }

                        this.DoeZet(nNnummer);
                        break;

                    case Events.IsGewonnen:
                        Console.WriteLine("Hoeraaaaa " + opgeknipt[1] + " is Gewonnen ");
                        break;

                    case Events.NiemandGewonnen:
                        Console.WriteLine("Niemand heeft gewonnen ...");
                        break;

                    case Events.HetIsBezit:
                        Console.WriteLine("Het is bezit of ongeldig nummer, je mag ander nummer geven.");
                        string nummerString3 = Console.ReadLine();
                        short num;
                        while (!short.TryParse(nummerString3, out num))
                        {
                            Console.WriteLine("Geef maar een nummer toe !!!");
                            nummerString = Console.ReadLine();
                        }

                        this.DoeZet(num);
                        break;

                    case Events.NieuwSpel:
                        string[] opgekniptNieuwe = opgeknipt[1].Split(new char[] { ',' });
                        Console.WriteLine("Andere speler heeft het spel verleten. Wil je nog een spel doen , J of N");
                        string nieuwSpel = Console.ReadLine();
                        nieuwSpel.ToLower();
                        while ((nieuwSpel != "j") && (nieuwSpel != "n"))
                        {
                            Console.WriteLine("Geef maar J of N !");
                            nieuwSpel = Console.ReadLine();
                        }

                        switch (nieuwSpel)
                        {
                            case "j":
                                short a = short.Parse(opgekniptDerdeDeel[1]);
                                this.VerzoekOmStartenSpel(opgekniptNieuwe[0], a);
                                break;
                            case "n":
                                Console.WriteLine("Tot ziens !!");
                                break;
                        }

                        break;

                    case Events.WachtenOpAndereDeelnemer:
                        if (opgeknipt.Length > 1)
                        {
                            Console.WriteLine(opgeknipt[1]);
                        }

                        Console.WriteLine("wachten op andere speler");
                        break;
                }
            }
            catch (Exception exp)
            {
                // ok dan krijg ik een foutmelding, stuur die dan terug
                returnMessage = EventHelper.CreateErrorEvent(exp);
            }

            return returnMessage;
        }

        private void SpelerGestart(string teken)
        {
            string message = CommandoHelper.CreateSpelerGestartCommando(teken);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }

        private void DoeZet(short nummer)
        {
            string message = CommandoHelper.DoeZet(nummer);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }

        private void NieuwRondje(string nieuwRondje)
        {
            string message = CommandoHelper.NieuwRondje(nieuwRondje);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }

        private void BeeindigSpel()
        {
            string message = CommandoHelper.BeeindigSpel();

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            this.StartListening(stream, this.tcpClient);
        }
    }
}
