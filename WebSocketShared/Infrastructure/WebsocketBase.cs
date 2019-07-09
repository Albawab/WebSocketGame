namespace HenE.WebSocketExample.Shared.Infrastructure
{
    using HenE.Abdul.GameOX;
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class WebsocketBase
    {
        /// <summary>
        /// Hier gaat de server starten.
        /// </summary>
        public async Task StartListeningAsync(/*NetworkStream stream,*/ TcpClient client)
        {
            while (true)
            {
                byte[] receivedBuffer = new byte[5000];

                // De stream tussen de client en de server.
                NetworkStream stream = client.GetStream();
                // Hier gaat de info lezen.
                await stream.ReadAsync(receivedBuffer, 0, receivedBuffer.Length);

                // Opslag de information die uit de client komet 
                StringBuilder msg = new StringBuilder();

                foreach (byte b in receivedBuffer)
                {
                    if (b.Equals(00))
                    {
                        break;
                    }
                    else
                    {
                        //voeg elk letter toe
                        msg.Append(Convert.ToChar(b).ToString());
                    }
                }

                string returnMessage = ProcessStream(msg.ToString(), client);

                // als de returnMessage wat zinvols heeft, dat terugsturen

            }
        }

        protected void ProcessReturnMessage(string returnMessage,  GameOX game, TcpClient sendingClient)
        {
            bool bericht = true;
            if (!String.IsNullOrWhiteSpace(returnMessage))
            {
                // naar welke clients moet ik die sturen?
                foreach(Speler speler in game.Spelers)
                {
                    if (speler.tcpClient != null && bericht)
                    {

                            SendMessageAsync(speler.tcpClient, returnMessage);

                       
                    }
                }
            }
        }



        protected  abstract string ProcessStream(string stream, TcpClient client);

        /// <summary>
        /// Die method stuur de info naar de cleinten.
        /// </summary>
        /// <param name="client">Een client.</param>
        /// <param name="message"> De information</param>
        protected NetworkStream SendMessage(TcpClient client, string message)
        {
            // Get hoeveel letter in de message als nummer.
            int byteCount = Encoding.ASCII.GetByteCount(message);
            byte[] sendData = new byte[byteCount];

            // Convert de message to byts
            sendData = Encoding.ASCII.GetBytes(message);

            //Open een stream tussen de server en de client
            NetworkStream stream = client.GetStream();

            //Stuur de info naar de client.
            stream.Write(sendData, 0, sendData.Length);
            return stream;
        }


        protected async Task<NetworkStream> SendMessageAsync(TcpClient client, string message)
        {
            // Get hoeveel letter in de message als nummer.
            int byteCount = Encoding.ASCII.GetByteCount(message);
            byte[] sendData = new byte[byteCount];

            // Convert de message to byts
            sendData = Encoding.ASCII.GetBytes(message);

            //Open een stream tussen de server en de client
            NetworkStream stream = client.GetStream();

            //Stuur de info naar de client.
            await stream.WriteAsync(sendData, 0, sendData.Length);

            return stream;
        }

    }
}
