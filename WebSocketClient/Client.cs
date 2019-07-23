namespace HenE.WebSocketExample.WebSocketClient
{
    using System;
    using System.Net.Sockets;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;

    public class Client : WebsocketBase
    {
        private TcpClient _tcpClient = null;

        public int ServerPort { get; private set; }

        public string Hostname { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="hostname"></param>
        /// <param name="serverPort"></param>
        public Client(string hostname, int serverPort)
        {
            this.Hostname = hostname;
            this.ServerPort = serverPort;
        }

        public void Start()
        {
            this._tcpClient = new TcpClient(this.Hostname, this.ServerPort);
        }

        public void Stop()
        {
            if (this._tcpClient != null)
            {
                this._tcpClient.Close();
            }
        }

        protected override string ProcessStream(string stream, TcpClient client)
        {
            // bepaal het event
            // het event is het gedeelte in de msg voor de #
            // daarna komen de parameters
            string eventParams = string.Empty;
            string returnMessage = null;
            Events events = EventHelper.SplitEventAndParamsFromMessage(stream, out eventParams);
            string[] opgeknipt = eventParams.Split(new char[] { '&' });
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
                        Console.WriteLine(opgeknipt[0] + " Welke teken wil je gebruiken ?");
                        break;

                    case Events.WachtenOpAndereDeelnemer:
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

        public void VerzoekOmStartenSpel(string spelersnaam, short dimension)
        {
            string message = CommandoHelper.CreateVerzoekTotDeelnemenSpelCommando(spelersnaam, dimension);

            NetworkStream stream = this.SendMessage(this._tcpClient, message);

            this.StartListeningAsync(stream, this._tcpClient);
        }

        /*
        public void WieStart()
        {
            String message = CommandoHelper.WieStart();

            NetworkStream stream = SendMessage(this._tcpClient, message);

            StartListeningAsync(stream, this._tcpClient);
        }
        */
    }
}
