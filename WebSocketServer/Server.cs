// <copyright file="Server.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using HenE.Abdul.Game_OX;
    using HenE.Abdul.GameOX;
    using HenE.Abdul.GameOX.Protocol;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;
    using HenE.WebSocketExample.WebSocketServer.CommandHandlers;

    /// <summary>
    /// De server.
    /// </summary>
    public class Server : WebsocketBase
    {
        // de lijst van de cliënten die contact hebben met de server
        private readonly TcpListener listener = null;
        private readonly SpelHandler spelHandler = new SpelHandler();
        private readonly List<GameOX> gameOXen = new List<GameOX>();
        private bool listening = false;
        private TcpClient tegeHuidigeCleint = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// Hier staat de constructor van de Server.
        /// </summary>
        /// <param name="ipAddress">IPAdres.</param>
        /// <param name="poort">De poort nummer.</param>
        public Server(IPAddress ipAddress, int poort)
        {
            this.IpAddress = ipAddress;
            this.Port = poort;

            // _listener = new TcpListener(/*this.IpAddress,*/ Port);
        }

        /// <summary>
        /// Gets de Poort waar kunnen de cliënt en server aansluiten.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets het IPAdress Van de client.
        /// </summary>
        public IPAddress IpAddress { get; private set; }

        /// <summary>
        /// Hier gaat de server starten.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("In the server");
            this.StartListener();
        }

        /// <summary>
        /// De server stopt hier.
        /// </summary>
        public void Stop()
        {
            Console.WriteLine("Stop de server");

            // check of de server wel is gestart
            this.listening = false;
            this.listener.Stop();
        }

        /// <summary>
        /// De server start hier.
        /// </summary>
        public async void StartListener() // non blocking listener
        {
            TcpListener listener = new TcpListener(/*this.IpAddress,*/ this.Port);
            listener.Start();
            this.listening = true;

            while (this.listening)
            {
                TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false); // non blocking waiting

                // We are already in the new task to handle this client...
                this.HandleClient(client, this);
            }
        }

        /// <summary>
        /// Process van de server.
        /// </summary>
        /// <param name="stream">Stream die uit de client komt.</param>
        /// <param name="client"> Client.</param>
        /// <returns>informatie.</returns>
        protected override string ProcessStream(string stream, TcpClient client)
        {
            // bepaal de opdracht
            // de opdracht is het gedeelte in de msg voor de #
            // daarna komen de parameters.
            string commandParams = string.Empty;
            string returnMessage = null;
            GameOX game = null;

            Commandos commando = CommandoHelper.SplitCommandAndParamsFromMessage(stream, out commandParams);

            foreach (GameOX games in this.gameOXen)
            {
                foreach (TcpClient tcp in games.TcpClients)
                {
                    if (tcp == client)
                    {
                        game = games;
                    }
                }
            }

            // Add een client to clientLijst.
            try
            {
                switch (commando)
                {
                    // Verdeel between de naam van de speler en de dimension van het bord
                    // wanneer de commandos is equal VerzoekTotDeelnemenSpel
                    case Commandos.VerzoekTotDeelnemenSpel:
                        VerzoekTotDeelnemenSpelCommandHandler handler = new VerzoekTotDeelnemenSpelCommandHandler(this.spelHandler, client);
                        returnMessage = handler.HandleFromMessage(commandParams, out game);
                        if (game.Status == GameOXStatussen.Gestart)
                        {
                            this.gameOXen.Add(game);
                            this.ProcessReturnMessage(returnMessage, game.TcpClients);
                            game.Start(client, game);
                        }
                        else
                        {
                            this.ProcessReturnMessage(returnMessage, client);
                        }

                        break;

                    case Commandos.SpelerGestart:
                        Teken teken = TekenHelper.CreateTekenEnum(commandParams);
                        TekenHelper.AddTekenToSpeler(teken, this.gameOXen, client);
                        returnMessage = EventHelper.CreateEvents(Events.YourTurn);
                        this.ProcessReturnMessage(returnMessage, client);

                        // stuur een wacht bericht naar andere speler.
                        returnMessage = this.spelHandler.TegeHuidigeClient(client, game.TcpClients, out this.tegeHuidigeCleint);
                        this.ProcessReturnMessage(returnMessage, this.tegeHuidigeCleint);
                        break;

                    case Commandos.StartSpel:
                       // returnMessage = EventHelper.CreateSpelerGestartEvent();
                      //  ProcessReturnMessage(returnMessage, tcpClients);
                        break;

                    case Commandos.DoeZet:
                        // stuur een wacht bericht naar andere speler.
                        returnMessage = this.spelHandler.TegeHuidigeClient(client, game.TcpClients, out this.tegeHuidigeCleint);
                        this.ProcessReturnMessage(returnMessage, this.tegeHuidigeCleint);
                        short nummer = short.Parse(commandParams);

                        // Handel de info van de speler
                        foreach (GameOX games in this.gameOXen)
                        {
                            foreach (Speler speler in games.Spelers)
                            {
                                if (speler.TcpClient == client)
                                {
                                    games.ChekOfHetValidBeZitIs(client, nummer, games);
                                }
                            }
                        }

                        break;

                    case Commandos.WachtenOpAndereDeelnemer:
                        this.ProcessReturnMessage(returnMessage, game.TcpClients);
                        break;

                    case Commandos.NieuwRondje:
                        foreach (GameOX games in this.gameOXen)
                        {
                            foreach (TcpClient tcp in games.TcpClients)
                            {
                                if (tcp == client)
                                {
                                    games.StartNieuwRondje(client, games);
                                }
                            }
                        }

                        break;

                    case Commandos.BeeindigSpel:
                        game.BeeidigSpel(game);
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

        /// <summary>
        /// Handel de client.
        /// </summary>
        /// <param name="client">client.</param>
        /// <param name="server">server.</param>
        private async void HandleClient(TcpClient client, Server server)
        {
            await server.StartListeningAsync(client.GetStream(), client);
        }
    }
}
