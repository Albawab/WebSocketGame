using HenE.WebSocketExample.Shared.Infrastructure;
using HenE.WebSocketExample.Shared.Protocol;
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
            // bepaal de opdracht
            // de opdracht is het gedeelte in de msg voor de #
            // daarna komen de parameters
            string commandParams = "";
            string returnMessage = null;

            // ik jkrijg hier een event

            //Commandos commando = CommandoHelper.SplitCommandAndParamsFromMessage(stream, out commandParams);

            try
            {

                /*                switch (commando)
                                {
                                    // Verdeel between de naam van de speler en de dimension van het bord
                                    //wanneer de commandos is equal VerzoekTotDeelnemenSpel
                                    case Commandos.VerzoekTotDeelnemenSpel:
                                        VerzoekTotDeelnemenSpelCommandHandler handler = new VerzoekTotDeelnemenSpelCommandHandler(_spelHandler, client);
                                        returnMessage = handler.HandleFromMessage(commandParams);
                                        break;
                                }
                */
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

            SendMessage(this._tcpClient, message);
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
