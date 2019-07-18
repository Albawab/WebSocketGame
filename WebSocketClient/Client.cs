﻿using HenE.WebSocketExample.Shared.Infrastructure;
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

            StartListeningAsync(stream, this._tcpClient);
        }
       
    }
}
