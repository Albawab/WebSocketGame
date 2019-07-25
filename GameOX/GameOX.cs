namespace HenE.Abdul.GameOX
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using HenE.Abdul.Game_OX;
    using HenE.WebSocketExample.Shared.Infrastructure;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    /// todo Abdul, alles koprieren uit de spel class.
    /// </summary>
    public class GameOX : WebsocketBase
    {
       public IList<Speler> Spelers = new List<Speler>();
       public List<TcpClient> TcpClients = new List<TcpClient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GameOX"/> class.
        /// </summary>
        /// <param name="dimension"></param>
       public GameOX(short dimension)
        {
            this.Dimension = dimension;
            this.Status = GameOXStatussen.NogNietGestart;
        }

       public string Naam { get; set; }

        /// <summary>
        ///
        /// </summary>
       public GameOXStatussen Status { get; private set; }

       public short Dimension { get; set; }

       public bool IsWaitingForGamers()
        {
            return this.Status == GameOXStatussen.Waiting;
        }

       public void AddPlayer(string naam, TcpClient client, short dimention)
        {
            this.Spelers.Add(new HumanSpeler(naam, dimention) { TcpClient = client });
        }

       public void WachtOpAndereSpeler()
        {
            this.Status = GameOXStatussen.Waiting;
        }

       protected override string ProcessStream(string stream, TcpClient client)
        {
            string returnMessage = null;
            this.ProcessReturnMessage(stream, client);

            return returnMessage;
        }

        /// <summary>
        /// we hebben genoeg spelers en alle informatie die we nodig hebben, we gaan starten.
        /// </summary>
       public void Start(TcpClient tcpClient)
        {
            string bord;
            string msg = string.Empty;

            // maak een bord, met de jusite dimension
            Bord huidigeBord = new Bord(this.Dimension);

            // ==> String . Teken het bord
            bord = huidigeBord.TekenBord();

            // hoe bepaal je wie mag beginnen?
            foreach (Speler speler in this.Spelers)
            {
                this.TcpClients.Add(speler.TcpClient);
                if (speler.TcpClient == tcpClient)
                {
                    msg = string.Format("{0}&{1}", EventHelper.CreateWachtenOpEenAndereDeelnemenEvent(), bord);
                    this.ProcessStream(msg, speler.TcpClient);
                }
                else
                {
                    msg = string.Format("{0}&{1}", EventHelper.CreateSpelerGestartEvent(), bord);
                    this.ProcessStream(msg, speler.TcpClient);
                    break;
                }
            }

            while (true)
            {
                foreach (Speler speler in this.Spelers)
                {
                    speler.SpelStartedHandler();
                }
            }
        }

        /// <summary>
        /// Bepaalt wie gaat tegen de huidige speler spelen.
        /// </summary>
        /// <param name="huidigeSpeler">De spelers.</param>
        /// <returns>De speler.</returns>
       public Speler TegenSpeler(Speler huidigeSpeler)
        {
            foreach (Speler speler in this.Spelers)
            {
                if (speler != huidigeSpeler)
                {
                    return speler;
                }
            }

            return null;
        }


        /// <summary>
        /// Deze method geef een nieuwe speler als de speler niet al bestaat.
        /// </summary>
        /// <param name="naam">De naam van de human speler.</param>
        /// <param name="teken">welek teken gaat de spelr gebruiken.</param>
        /// <returns>Deze method geeft een neuwie speler terug.</returns>
       public Speler AddHumanSpeler(string naam, Teken teken, short dimension)
        {
            // bestaat deze speler al?
            if (this != null)
            {
                throw new ArgumentException("Speler bestaat al");
            }

            Speler speler = new HumanSpeler(naam, dimension)
            {
                TeGebruikenTeken = teken,
            };
            this.Spelers.Add(speler);

            return speler;
        }

        /// <summary>
        /// Deze method zoekt of de naam van de niuwe speler al bastaat.
        /// </summary>
        /// <param name="speler">de speler.</param>
        /// <returns>De niuwe speler.</returns>
       public string FindSpelerByNaam(Speler speler)
        {
                if (this.TegenSpeler(speler).Naam == speler.Naam)
                {
                    return speler.Naam;
                }

                return null;
        }
    }
}
