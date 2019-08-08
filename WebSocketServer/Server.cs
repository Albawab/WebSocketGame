// <copyright file="Server.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
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
        //private readonly List<GameOX> gameOXen = new List<GameOX>();
        private TcpClient tegenHuidigeClient = null;

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
        }

        /// <summary>
        /// Gets de Poort waar kunnen de cliënt en server aansluiten.
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets het IPAdress Van de client.
        /// </summary>
        public IPAddress IpAddress { get; private set; }

        private bool Listening { get; set; } = false;

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
            this.Listening = false;
            this.listener.Stop();
        }

        /// <summary>
        /// De server start hier.
        /// </summary>
        public void StartListener() // non blocking listener
        {
            TcpListener listener = new TcpListener(IPAddress.Any, this.Port);
            listener.Start();
            this.Listening = true;

            while (this.Listening)
            {
                /*TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false); // non blocking waiting*/

                TcpClient client = listener.AcceptTcpClient();

                if (client != null)
                {
                    Console.WriteLine($"Connected {client.Client.LocalEndPoint}:{client.Client.LocalEndPoint}  ");
                }

                // We are already in the new task to handle this client...
                this.HandleClientAsync(client, this);
            }
        }

        /// <summary>
        /// Process van de server.
        /// </summary>
        /// <param name="stream">Stream die uit de client komt.</param>
        /// <param name="client"> Client.</param>
        /// <returns>informatie.</returns>
        /// <exception cref="System.InvalidOperationException">Wordt gegooid wanneer een commando wordt aangeroepen op een ongeldig moment.</exception>
        protected override string ProcessStream(string stream, TcpClient client)
        {
            // bepaal de opdracht
            // de opdracht is het gedeelte in de msg voor de #
            // daarna komen de parameters.
            string commandParams = string.Empty;
            string returnMessage = null;
            GameOX game = null;

            Commandos commando = CommandoHelper.SplitCommandAndParamsFromMessage(stream, out commandParams);

            /*foreach (GameOX games in this.gameOXen)
            {
                foreach (TcpClient tcp in games.TcpClients)
                {
                    if (tcp == client)
                    {
                        game = games;
                    }
                }
            }*/

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

                    case Commandos.SpelTegenComputer:
                        VerzoekTotDeelnemenSpelCommandHandler handlerTegenComputerSpelen = new VerzoekTotDeelnemenSpelCommandHandler(this.spelHandler, client);
                        returnMessage = handlerTegenComputerSpelen.HandleFromMessage(commandParams, out game);
                        this.ProcessReturnMessage(returnMessage, client);
                        this.gameOXen.Add(game);
                        game.Start(client, game);
                        break;

                    case Commandos.SpelerGestart:
                        Teken teken = TekenHelper.CreateTekenEnum(commandParams);
                        TekenHelper.AddTekenToSpeler(teken, this.gameOXen, client);
                        returnMessage = EventHelper.CreateEvents(Events.YourTurn);
                        this.ProcessReturnMessage(returnMessage, client);

                        // stuur een wacht bericht naar andere speler.
                        returnMessage = this.spelHandler.TegeHuidigeClient(client, game.TcpClients, out this.tegenHuidigeClient);
                        this.ProcessReturnMessage(returnMessage, this.tegenHuidigeClient);
                        break;

                    case Commandos.StartSpel:
                        // returnMessage = EventHelper.CreateSpelerGestartEvent();
                        //  ProcessReturnMessage(returnMessage, tcpClients);
                        break;

                    case Commandos.DoeZet:
                        // stuur een wacht bericht naar andere speler.
                        returnMessage = this.spelHandler.TegeHuidigeClient(client, game.TcpClients, out this.tegenHuidigeClient);
                        this.ProcessReturnMessage(returnMessage, this.tegenHuidigeClient);
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
                        if (game == null)
                        {
                            throw new InvalidOperationException($"Het commando {nameof(Commandos.WachtenOpAndereDeelnemer)} mag niet worden gebruikt wanneer er nog geen game is gestart.");
                        }

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
                                    break;
                                }
                            }
                        }

                        break;

                    case Commandos.BeeindigSpel:
                        game?.BeeidigSpel(client);
                        this.gameOXen.Remove(game);
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
        private async Task HandleClientAsync(TcpClient tcpClient, Server server)
        {
            try
            {
                await server.StartListeningAsync(tcpClient.GetStream(), tcpClient);
                // server.StartListening(client.GetStream(), client);
            }
            catch
            {
                GameOX gameToDelete = null;

                foreach (GameOX game in this.gameOXen)
                {
                    foreach (Speler speler in game.Spelers)
                    {
                        if (tcpClient == speler.TcpClient)
                        {
                            game.Annuleer(speler);
                            if (game.Status == GameOXStatussen.Finished)
                            {
                                // dan heb ik geen spelers meer bij deze game
                                // game mag verwijderd worden
                                // maar ik mag niet verwijderen in een foreach
                                gameToDelete = game;
                            }
                            break;
                        }
                    }
                }

                if (gameToDelete != null)
                {
                    this.gameOXen.Remove(gameToDelete);
                }
            }
        }
    }
}