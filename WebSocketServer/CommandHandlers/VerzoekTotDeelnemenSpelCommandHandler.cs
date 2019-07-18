namespace HenE.WebSocketExample.WebSocketServer.CommandHandlers
{
    using System;
    using System.Net.Sockets;
    using HenE.Abdul.GameOX;
    using HenE.WebSocketExample.Shared.Commands;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    /// 
    /// </summary>
    public class VerzoekTotDeelnemenSpelCommandHandler : CommandHandler
    {
        readonly SpelHandler spelHandler;
        readonly TcpClient tcpClient;

        public VerzoekTotDeelnemenSpelCommandHandler(SpelHandler spelHandler, TcpClient tcpClient)
        {
            this.spelHandler = spelHandler;
            this.tcpClient = tcpClient;

        }

        /// <summary>
        /// functie die de parameters in string krijgt en die string opknipt in de params die nodig zijn
        /// </summary>
        /// <param name="messageParams">params uit het bericht, gedeelte na de # en gescheiden door &</param>
        public string HandleFromMessage(string messageParams, out GameOX game)
        {
            string[] opgeknipt = messageParams.Split(new char[] { '&' });

            // lengte van de arrayu moet 
            // 0 moet naam zijn
            // 1 moet dimension zijn
            if (opgeknipt.Length != 2)
            {
                // andere foutmelding
                throw new ArgumentException("U hebt geen nummer ingevoerd");
            }

            // Als  de naam leeg is dan mag niet door gaan
            if (String.IsNullOrWhiteSpace(opgeknipt[0]))
            {
                throw new ArgumentException("Er staat geen naam, Voeg een naam.");                
            }
            
            //Probeer te omzetten string to nummer  
            Int16 dimension = 0;
            if (!Int16.TryParse(opgeknipt[1],out dimension))
            {
                throw new ArgumentOutOfRangeException("U hebt geen nummer ingevoerd");
            }

            // Als het grootre dan 9 of kleiner dan 2 is mag dan niet door gaan.
            if (dimension < 2 || dimension > 9)
            {
                throw new ArgumentOutOfRangeException("U hebt geen nummer ingevoerd");
            }
            
            return Handle(opgeknipt[0], dimension, out game);
        }


        /// <summary>
        /// handle het command af
        /// </summary>
        /// <param name="spelersnaam"> naam van de speler</param>
        /// <param name="dimension">dimension van het spel</param>
        /// <return>de message die gereturnd moet worden </return>
        public string Handle(string spelersnaam, short dimension, out GameOX game)
        {
            string returnMessage = "";
            game = null;
            // zoek in de spelHandler of er al een open spel is met die dimension
            game = this.spelHandler.GetOpenSpelbyDimension(dimension);
            // zo ja, voeg speler toe en start spel
            if (game != null)
            {
                game.AddPlayer(spelersnaam, tcpClient, dimension);

                game.Start(spelersnaam, tcpClient, dimension);
                // oke het spel kan beginnen

                //returnMessage = EventHelper.CreateSpelgestartEvent(game);

                return returnMessage = " we gaan starten naar iedereen";  

                // // bepaal wie er gaat beginnen
                /// voor nu, speler 1 begint
                ///  stuur naar speler 1 bericht dat hij moet beginnen
                ///  stuur naar speler 2 bericht dat hij moet wachten


            }
            else
            {
                // nee, maak spel aan 
                // voeg de speler toe
                //en zet op de wachtlijst 
                game = this.spelHandler.CreateGame(dimension, spelersnaam, tcpClient);

                game.WachtOpAndereSpeler();

                // de speler moet wachten op een andere
                returnMessage = EventHelper.CreateWachtenOpEenAndereDeelnemenEvent();
            }

            // wat wil ik nu terugsturen?
            //Commandos.StartSpel), spelersnamen.ToString()
            return returnMessage;
        }

    }
}
