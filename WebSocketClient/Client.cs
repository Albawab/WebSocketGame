// <copyright file="Client.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketClient
{
    using System;
    using System.Net.Sockets;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    /// Client.
    /// </summary>
    public class Client : WebsocketBase
    {
        private TcpClient tcpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="hostname">Host naam.</param>
        /// <param name="serverPort"> Server Port.</param>
        public Client(string hostname, int serverPort)
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
        public string Hostname { get; private set; }

        /// <summary>
        /// Het client start.
        /// </summary>
        public void Start()
        {
            this.tcpClient = new TcpClient(this.Hostname, this.ServerPort);
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
        public async void VerzoekOmStartenSpel(string spelersnaam, short dimension)
        {
            string message = CommandoHelper.CreateVerzoekTotDeelnemenSpelCommando(spelersnaam, dimension);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            await this.StartListeningAsync(stream, this.tcpClient);
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
                        Console.WriteLine("We gaan starten " + opgeknipt[0] + " Tegen " + opgeknipt[1] + " De dimenstion is : " + opgeknipt[2]);
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
                        Console.WriteLine(opgekniptDerdeDeel[1] + " Is gewonnen !!!!");
                        Console.WriteLine(opgekniptDerdeDeel[1] + " heeft " + opgekniptDerdeDeel[0] + "Punten !");
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
                        Console.WriteLine(eventParams);
                        Console.WriteLine("Geef een nummer toe !!");
                        short nNnummer = short.Parse(Console.ReadLine());
                        this.DoeZet(nNnummer);
                        break;

                    case Events.IsGewonnen:
                        Console.WriteLine("Hoeraaaaa " + opgeknipt[1] + " is Gewonnen ");
                        break;

                    case Events.HetIsBezit:
                        Console.WriteLine("Het is bezit , je mag andre nummer geven.");
                        short num = short.Parse(Console.ReadLine());
                        this.DoeZet(num);
                        break;

                    case Events.WachtenOpAndereDeelnemer:
                        Console.WriteLine(eventParams);
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

        private async void SpelerGestart(string teken)
        {
            string message = CommandoHelper.CreateSpelerGestartCommando(teken);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            await this.StartListeningAsync(stream, this.tcpClient);
        }

        private async void DoeZet(short nummer)
        {
            string message = CommandoHelper.DoeZet(nummer);

            NetworkStream stream = await this.SendMessageAsync(this.tcpClient, message);

            await this.StartListeningAsync(stream, this.tcpClient);
        }

        private async void NieuwRondje(string nieuwRondje)
        {
            string message = CommandoHelper.NieuwRondje(nieuwRondje);

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            await this.StartListeningAsync(stream, this.tcpClient);
        }

        private async void BeeindigSpel()
        {
            string message = CommandoHelper.BeeindigSpel();

            NetworkStream stream = this.SendMessage(this.tcpClient, message);

            await this.StartListeningAsync(stream, this.tcpClient);
        }
    }
}
