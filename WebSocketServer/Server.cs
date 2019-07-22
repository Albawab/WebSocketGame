using HenE.Abdul.GameOX;
using HenE.WebSocketExample.Shared.Infrastructure;
using HenE.WebSocketExample.Shared.Protocol;
using HenE.WebSocketExample.WebSocketServer.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HenE.WebSocketExample.WebSocketServer
{
    public class Server : WebsocketBase
    {

        // de lijst van de cliënten die contact hebben met de server
        private readonly List<TcpClient> tcpClients = new List<TcpClient>();

        private TcpListener _listener = null;


        // De Poort waar kunnen de cliënt en server aansluiten.
        public int Port { get; private set; }

        // Het IPAdress Van de client
        public IPAddress IpAddress { get; private set; }


        private readonly SpelHandler _spelHandler = new SpelHandler();

        private bool _listening = false;

        /// <summary>
        /// Hier staat de constructor van de Server 
        /// </summary>
        /// <param name="ipAddress">IPAdres</param>
        /// <param name="poort">De poort nummer</param>
        public Server(IPAddress ipAddress, int poort)
        {
            IpAddress = ipAddress;
            Port = poort;

            //_listener = new TcpListener(/*this.IpAddress,*/ Port); 
        }

        /// <summary>
        /// Hier gaat de server starten.
        /// </summary>
        public void Start()
        {            
            Console.WriteLine("In the server");
            StartListener();
        }

        public void Stop()
        {
            Console.WriteLine("Stop de server");

            // check of de server wel is gestart
            _listening = false;
            _listener.Stop();
        }


        public async void StartListener() //non blocking listener
        {
            TcpListener listener = new TcpListener(/*this.IpAddress,*/ Port);
            listener.Start();
            _listening = true;

            while (_listening)
            {
                TcpClient client = await listener.AcceptTcpClientAsync().ConfigureAwait(false);//non blocking waiting                    
                                                                                                // We are already in the new task to handle this client...   
                HandleClientAsync(client, this);
                //StartListening( client);
                int u = 0;
            }
        }


        private async Task HandleClientAsync(TcpClient client, Server server)
        {

                //var client = (TcpClient)obj;
                server.StartListeningAsync(client.GetStream(), client);
                //return Task.CompletedTask;
            
        }




        protected override string ProcessStream(string stream, TcpClient client)
        {
            // bepaal de opdracht
            // de opdracht is het gedeelte in de msg voor de #
            // daarna komen de parameters
            GameOX game = null;
            string commandParams = "";
            string returnMessage = null;

            Commandos commando = CommandoHelper.SplitCommandAndParamsFromMessage(stream, out commandParams);
            
            // Add een client to clientLijst.           
            try
            {

                switch (commando)
                {
                    // Verdeel between de naam van de speler en de dimension van het bord
                    //wanneer de commandos is equal VerzoekTotDeelnemenSpel
                    case Commandos.VerzoekTotDeelnemenSpel:
                        VerzoekTotDeelnemenSpelCommandHandler handler = new VerzoekTotDeelnemenSpelCommandHandler(_spelHandler, client);                        
                        returnMessage = handler.HandleFromMessage(commandParams, out game);
                        tcpClients.Add(client);
                        ProcessReturnMessage(returnMessage, game, tcpClients);
                        break;
                        
                    case Commandos.StartSpel:
                        tcpClients.Clear();
                        tcpClients.Add(client);
                        returnMessage = EventHelper.CreateSpelerGestartEvent();
                        ProcessReturnMessage(returnMessage, game, tcpClients);
                        break;
                    case Commandos.VerlaatSpel:

                        break;

                    case Commandos.WachtenOpAndereDeelnemer:

                        tcpClients.Add(client);
                        ProcessReturnMessage(returnMessage, game, tcpClients);
                        tcpClients.Clear();

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
    }
}
