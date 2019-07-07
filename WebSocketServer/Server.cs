using HenE.WebSocketExample.Shared.Infrastructure;
using HenE.WebSocketExample.Shared.Protocol;
using HenE.WebSocketExample.WebSocketServer.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace HenE.WebSocketExample.WebSocketServer
{
    public class Server : WebsocketBase
    {

        // de lijst van de cliënten die contact hebben met de server
        private List<TcpClient> tcpClients = new List<TcpClient>();

        private TcpListener _listener = null;


        // De Poort waar kunnen de cliënt en server aansluiten.
        public int Port { get; private set; }

        // Het IPAdress Van de client
        public IPAddress IpAddress { get; private set; }


        private SpelHandler _spelHandler = new SpelHandler();

        /// <summary>
        /// Hier staat de constructor van de Server 
        /// </summary>
        /// <param name="ipAddress">IPAdres</param>
        /// <param name="poort">De poort nummer</param>
        public Server(IPAddress ipAddress, int poort)
        {
            IpAddress = ipAddress;
            Port = poort;

            _listener = new TcpListener(/*this.IpAddress,*/ Port); 
        }

        /// <summary>
        /// Hier gaat de server starten.
        /// </summary>
        public void Start()
        {
            Console.WriteLine("In the server");
            _listener.Start();


            StartListening(_listener.AcceptTcpClient());

        }

        protected override string ProcessStream(string stream, TcpClient client)


        {
            // bepaal de opdracht
            // de opdracht is het gedeelte in de msg voor de #
            // daarna komen de parameters
            string commandParams = "";
            string returnMessage = null;
            Commandos commando = CommandoHelper.SplitCommandAndParamsFromMessage(stream, out commandParams);
            
            // Add een client to clientLijst.
            tcpClients.Add(client);

            try
            {

                switch (commando)
                {
                    // Verdeel between de naam van de speler en de dimension van het bord
                    //wanneer de commandos is equal VerzoekTotDeelnemenSpel
                    case Commandos.VerzoekTotDeelnemenSpel:
                        VerzoekTotDeelnemenSpelCommandHandler handler = new VerzoekTotDeelnemenSpelCommandHandler(_spelHandler, client);
                        returnMessage = handler.HandleFromMessage(commandParams);
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
