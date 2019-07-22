using System.Collections.Generic;
using System.Net.Sockets;
using System;
using HenE.Abdul.Game_OX;

namespace HenE.Abdul.GameOX
{
    /// <summary>
    /// todo Abdul, alles koprieren uit de spel class
    /// </summary>
    public class GameOX
    {
       
       public IList<Speler> Spelers = new List<Speler>();


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

        public void AddPlayer(string naam, TcpClient client,short dimention)
        {
            Spelers.Add(new HumanSpeler(naam, dimention) { TcpClient = client });
        }

        public void WachtOpAndereSpeler()
        {
            this.Status = GameOXStatussen.Waiting;
        }


        /// <summary>
        /// we hebben genoeg spelers en alle informatie die we nodig hebben, we gaan starten
        /// </summary>
        public void Start(string spelersnaam,TcpClient tcpClient, short dimension)
        {

            foreach (Speler speler in this.Spelers)
            {
                if (FindSpelerByNaam(speler) == speler.Naam)
                {
                    speler.Naam = speler.Naam + 1;
                }
                break;
            }
            
            // maak een bord, met de jusite dimension
            Bord huidigeBord = new Bord(dimension);

             string bord = huidigeBord.TekenBord(); //==> String . Het bord

            // hoe bepaal je wie mag beginnen?


            foreach (Speler speler in this.Spelers)
            {
                speler.SpelStartedHandler();
            }
            //this._huidigeBord = new Bord(dimension, this);
            //Teken teken = new Teken();

            //mess: We gaan starten; Naar allemaal  

            //returnMessage = EventHelper.CreateSpelgestartEvent(game);




            // wie begint?
            //this._huidigeBord.TekenBord();         

        }

        /// <summary>
        /// Bepaalt wie gaat tegen de huidige speler spelen.
        /// </summary>
        /// <param name="huidigeSpeler">huidigeSpeler.</param>
        /// <param name="spelers">De spelers.</param>
        /// <returns>De speler.</returns>
        public Speler TegenSpeler(Speler huidigeSpeler)
        {
            foreach (Speler speler in Spelers)
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
        /// <param name="naam">De naam van de speler.</param>
        /// <returns>De niuwe speler.</returns>
        public string FindSpelerByNaam(Speler speler)
        {
                if (TegenSpeler(speler).Naam == speler.Naam)
                {
                    return speler.Naam; ;
                }

            return null;
        }

    }
}
