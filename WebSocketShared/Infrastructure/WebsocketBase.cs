// <copyright file="WebsocketBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.Shared.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// De class van de web socket.
    /// </summary>
    public abstract class WebsocketBase
    {
        /// <summary>
        /// Hier gaat de server starten.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <param name="stream">De stream.</param>
        /// <param name="client">De client.</param>
        public async Task StartListeningAsync(NetworkStream stream, TcpClient client)
        {
            while (true)
            {
                byte[] receivedBuffer = new byte[5000];

                // De stream tussen de client en de server.
                stream = client.GetStream();

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
                        // voeg elk letter toe
                        msg.Append(Convert.ToChar(b).ToString());
                    }
                }

                string returnMessage = this.ProcessStream(msg.ToString(), client);

                // als de returnMessage wat zinvols heeft, dat terugsturen
            }
        }

        /// <summary>
        /// Resend the message to two cleints.
        /// </summary>
        /// <param name="returnMessage">De message.</param>
        /// <param name="receivingClients">List of the clients.</param>
        protected async void ProcessReturnMessage(string returnMessage, List<TcpClient> receivingClients)
        {
            if (!string.IsNullOrWhiteSpace(returnMessage))
            {
                foreach (TcpClient client in receivingClients)
                {
                  await this.SendMessageAsync(client, returnMessage);
                }
            }
        }

        /// <summary>
        /// Resend the message to one client.
        /// </summary>
        /// <param name="returnMessage">De message.</param>
        /// <param name="receivingClient">De client die de massage naar hem gaat leveren. </param>
        protected async void ProcessReturnMessage(string returnMessage, TcpClient receivingClient)
        {
            if (!string.IsNullOrWhiteSpace(returnMessage))
            {
               await this.SendMessageAsync(receivingClient, returnMessage);
            }
        }

        /// <summary>
        /// De process stream tussen de server en de client.
        /// </summary>
        /// <param name="stream">De stream.</param>
        /// <param name="client">De client.</param>
        /// <returns>De message.</returns>
        protected abstract string ProcessStream(string stream, TcpClient client);

        /// <summary>
        /// Die method stuur de info naar de cleinten.
        /// </summary>
        /// <param name="client">Een client.</param>
        /// <param name="message"> De information.</param>
        /// <returns>Stream.</returns>
        protected NetworkStream SendMessage(TcpClient client, string message)
        {
            // Get hoeveel letter in de message als nummer.
            int byteCount = Encoding.ASCII.GetByteCount(message);
            byte[] sendData = new byte[byteCount];

            // Convert de message to byts
            sendData = Encoding.ASCII.GetBytes(message);

            // Open een stream tussen de server en de client
            NetworkStream stream = client.GetStream();

            // Stuur de info naar de client.
            stream.Write(sendData, 0, sendData.Length);

            return stream;
        }

        /// <summary>
        /// Die stuur de berecht af.
        /// </summary>
        /// <param name="client">client.</param>
        /// <param name="message">message.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected async Task<NetworkStream> SendMessageAsync(TcpClient client, string message)
        {
            // Get hoeveel letter in de message als nummer.
            int byteCount = Encoding.ASCII.GetByteCount(message);
            byte[] sendData = new byte[byteCount];

            // Convert de message to byts
            sendData = Encoding.ASCII.GetBytes(message);

            // Open een stream tussen de server en de client
            NetworkStream stream = client.GetStream();

            // Stuur de info naar de client.
            await stream.WriteAsync(sendData, 0, sendData.Length);
            return stream;
        }
    }
}
