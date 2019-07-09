using HenE.WebSocketExample.Shared.Infrastructure;
using HenE.WebSocketExample.Shared.Protocol;
using HenE.WebSocketExample.WebSocketServer.CommandHandlers;
using System;
using System.Net.Sockets;
using System.Text;

namespace HenE.WebSocketExample.WebSocketClient
{
    public class Client : WebsocketBase
    {
        private TcpClient _tcpClient = null;

        public int ServerPort { get; private set; }
        public String Hostname { get; private set; }

        public Client(String hostname, int serverPort)
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
            string eventParams = "";
            string returnMessage = null;
            TcpClient clientStart;
            Events bericht =Events.NotDefined;
            Events commando = EventHelper.SplitEventAndParamsFromMessage(stream, out eventParams);
            string[] opgeknipt = eventParams.Split(new char[] { '&' });
            try
            {
                switch (commando)
                {
                    case Events.Error:
                        Console.WriteLine(eventParams);
                        break;
                    case Events.SpelGestart:
                        Console.WriteLine("We gaan starten " + opgeknipt[0] + " Tegen " + opgeknipt[1] + " De dimenstion is : " + opgeknipt[2]);

                        if (Events.Bericht == bericht)
                        {
                            Console.WriteLine("Wil je Starten ? {0} of {1}", opgeknipt[0], opgeknipt[1]);

                            Console.ReadKey();
                            string wieStarten = Console.ReadLine();
                            while (wieStarten.ToLower() != opgeknipt[0].ToLower() && wieStarten.ToLower() != opgeknipt[1].ToLower())
                            {
                                if (wieStarten.ToLower() == opgeknipt[0].ToLower())
                                {

                                }
                                else if (wieStarten.ToLower() == opgeknipt[0].ToLower())
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Mag een juiste naam geven ?");
                                    wieStarten = Console.ReadLine();
                                }
                            }
                        }
                       

                        break;
                    case Events.WachtenOpAndereDeelnemer:
                        
                        Console.WriteLine("wachten op andere speler");
                        bericht = EventHelper.CreateEenEvent("Bericht");
                        clientStart = client;
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



        public void VerzoekOmStartenSpel(String spelersnaam, short dimension)
        {
            String message = CommandoHelper.CreateVerzoekTotDeelnemenSpelCommando(spelersnaam, dimension);

            NetworkStream stream = SendMessage(this._tcpClient, message);

            StartListeningAsync(/*stream,*/ this._tcpClient);
        }
        /*        private void SendMessage(string message)
                {
                    Console.WriteLine("versturen bericht " + message);

                    int byteCount = Encoding.ASCII.GetByteCount(message);
                    byte[] sendData = new byte[byteCount];
                    sendData = Encoding.ASCII.GetBytes(message);
                    NetworkStream stream = this._tcpClient.GetStream();
                    stream.Write(sendData, 0, sendData.Length);

                    GetThenListnaam(_tcpClient, stream);
                }

                public void GetThenListnaam(TcpClient client, NetworkStream stream)
                {
                    while (true) {
                        while (stream.DataAvailable) {
                            byte[] receivedBuffer = new byte[client.Available];
                            stream = client.GetStream();
                            stream.Read(receivedBuffer, 0, receivedBuffer.Length);
                            StringBuilder msg = new StringBuilder();

                            foreach (byte b in receivedBuffer)
                            {
                                if (b.Equals(00))
                                {
                                    break;
                                }
                                else
                                {
                                    msg.Append(Convert.ToChar(b).ToString());
                                }
                            }
                            Console.WriteLine(" terug naam " + msg.ToString() + msg.Length);
                        }
                    }
                    Console.ReadKey();
                }
        */
    }
}
