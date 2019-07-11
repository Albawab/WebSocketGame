using System.Collections.Generic;
using System.Net.Sockets;
using System;

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
            Spelers.Add(new HumanSpeler(naam, dimention) { tcpClient = client });
        }

        public void WachtOpAndereSpeler()
        {
            this.Status = GameOXStatussen.Waiting;
        }


        /// <summary>
        /// we hebben genoeg spelers en alle informatie die we nodig hebben, we gaan starten
        /// </summary>
        public void Start()
        {
            // maak een bord, met de jusite dimension
            // hoe bepaal je wie mag beginnen?

            //this._huidigeBord = new Bord(dimension, this);
            //Teken teken = new Teken();

            //mess: We gaan starten; Naar allemaal  
            foreach (Speler speler in this.Spelers)
            {
                speler.SpelStartedHandler();
            }
            //returnMessage = EventHelper.CreateSpelgestartEvent(game);


            // wie begint?
            //this._huidigeBord.TekenBord();
            /*Speler huidigeSpeler = this._spelers[this.wieStart];

            //mess: U mag beginnen; Naar huidige speler
            //mess: Wachten op speler 1; naar de andere speler; 
            while (!this.stopDeSpel)
            {
                List<short> vrijVelden = this._huidigeBord.VrijVelden();

                // teken het bord
                // vraag aan speler 1 wat hij wil doen
                huidigeSpeler.Zet(this._huidigeBord);
                //mess: wat wilt u doen?, dit zijn de keuzes; Naar huidige speler

                //Console.WriteLine();

                //this._huidigeBord.TekenBord();
                if (this._huidigeBord.HeeftTekenGewonnen(huidigeSpeler.TeGebruikenTeken))
                {
                  //  Console.WriteLine();
//                    Console.WriteLine(huidigeSpeler.Naam + " : Hoeraaaa " + huidigeSpeler.Naam + " je bent gewonnen !!!!");
                    //Console.WriteLine();
                    huidigeSpeler.BeeindigBord(this._huidigeBord);
                    //Console.WriteLine(huidigeSpeler.Naam + " Je hebt : " + huidigeSpeler.Punten + " Punt !!");
                    this.VraagNieuwRondje(huidigeSpeler);
                }

                if (this._huidigeBord.IsBordFinished())
                {
                    this.stopDeSpel = true;
                    //Console.WriteLine("Het boord is vol !!!");
                }

                huidigeSpeler = this.TegenSpeler(huidigeSpeler);
            }

            if (this.vraagEenRondje)
            {
                this.VraagNieuwRondje(huidigeSpeler);
            }

            return this._huidigeBord;
            */




        }

    }
}
