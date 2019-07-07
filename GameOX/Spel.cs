using System;
using System.Collections.Generic;

namespace HenE.Abdul.GameOX
{
    /// <summary>
    ///
    /// </summary>
    public class Spel
    {
        private IList<GameOX> _spelers = new List<GameOX>();
        private Bord _huidigeBord;
        private bool stopDeSpel = false;
        private bool vraagEenRondje = true;
        private int wieStart = 0;

        /// <summary>
        /// Dit start het spel.
        /// </summary>
        /// <param name="dimension">Het formaat van het bord.</param>
        /// <returns>Het getekende bord.</returns>
        public Bord Start(short dimension)
        {
            // maak een bord, met de jusite dimension
            // hoe bepaal je wie mag beginnen?
            this._huidigeBord = new Bord(dimension, this);
            Teken teken = new Teken();

            // wie begint?
            this._huidigeBord.TekenBord();
            this.WieStart();
            GameOX huidigeSpeler = this._spelers[this.wieStart];

            while (!this.stopDeSpel)
            {
                List<short> vrijVelden = this._huidigeBord.VrijVelden();

                // teken het bord
                // vraag aan speler 1 wat hij wil doen
                Console.WriteLine();
                huidigeSpeler.Zet(this._huidigeBord);
                this._huidigeBord.TekenBord();
                if (this._huidigeBord.HeeftTekenGewonnen(huidigeSpeler.TeGebruikenTeken))
                {
                    Console.WriteLine();
                    Console.WriteLine(huidigeSpeler.Naam + " : Hoeraaaa " + huidigeSpeler.Naam + " je bent gewonnen !!!!");
                    Console.WriteLine();
                    huidigeSpeler.BeeindigBord(this._huidigeBord);
                    Console.WriteLine(huidigeSpeler.Naam + " Je hebt : " + huidigeSpeler.Punten + " Punt !!");
                    this.VraagNieuwRondje(huidigeSpeler);
                }

                if (this._huidigeBord.IsBordFinished())
                {
                    this.stopDeSpel = true;
                    Console.WriteLine("Het boord is vol !!!");
                }

                huidigeSpeler = this.TegenSpeler(huidigeSpeler);
            }

            if (this.vraagEenRondje)
            {
                this.VraagNieuwRondje(huidigeSpeler);
            }

            return this._huidigeBord;
        }

        private void WieStart()
        {
            Console.WriteLine("  Wil je starten , J of N ?");
            string deEersteSpeler = Console.ReadLine().ToLower();
            while ((deEersteSpeler != "j") && (deEersteSpeler != "n"))
            {
                Console.WriteLine("Geef J of N !");
                deEersteSpeler = Console.ReadLine();
            }

            if (deEersteSpeler == "n")
            {
                this.wieStart = 1;
            }
        }

        private void VraagNieuwRondje(Speler huidigeSpeler)
        {
            Console.WriteLine("Wil je nog een rondje , J of N?");
            string nieuwRondjes = Console.ReadLine();
            nieuwRondjes.ToLower();
            while ((nieuwRondjes != "j") && (nieuwRondjes != "n"))
            {
                Console.WriteLine("Geef J of N !");
                nieuwRondjes = Console.ReadLine();
            }

            if (nieuwRondjes == "j")
            {
                this.stopDeSpel = false;
                this._huidigeBord.ResetBord();
                this.NeiuwRondje();
            }
            else
            {
                this.stopDeSpel = true;
                this.vraagEenRondje = false;
                this.DeWinner(huidigeSpeler);
            }
        }

        private void NeiuwRondje()
        {
            this.Start(this._huidigeBord.Dimension);
        }

        public GameOX TegenSpeler(GameOX huidigeSpeler)
        {
            foreach (GameOX speler in this._spelers)
            {
                if (speler != huidigeSpeler)
                {
                    return speler;
                }
            }

            return null;
        }

        private void DeWinner(GameOX huidigeSpeler)
        {
            if (huidigeSpeler.Punten > this.TegenSpeler(huidigeSpeler).Punten)
            {
                Console.WriteLine(huidigeSpeler.Naam + " is gewonnen ....");
            }
            else if (huidigeSpeler.Punten == this.TegenSpeler(huidigeSpeler).Punten)
            {
                Console.WriteLine("Niemand is gewonnen !");
            }
        }

        /// <summary>
        /// Deze method geef een nieuwe speler als de speler niet al bestaat.
        /// </summary>
        /// <param name="naam">De naam van de human speler.</param>
        /// <param name="teken">welek teken gaat de spelr gebruiken.</param>
        /// <returns>Deze method geeft een neuwie speler terug.</returns>
        public GameOX AddHumanSpeler(string naam, Teken teken)
        {
            // bestaat deze speler al?
            if (this.FindSpelerByNaam(naam) != null)
            {
                throw new Exception("Speler bestaat al");
            }

            GameOX speler = new HumanSpeler(naam);
            speler.TeGebruikenTeken = teken;
            this._spelers.Add(speler);

            return speler;
        }

        public GameOX AddComputerSpeler(string naam, Teken teken)
        {
            GameOX computerSpeler = new ComputerSpeler(naam);
            computerSpeler.TeGebruikenTeken = teken;
            this._spelers.Add(computerSpeler);

            return computerSpeler;
        }

        /// <summary>
        /// Deze method zoekt of de naam van de niuwe speler al bastaat.
        /// </summary>
        /// <param name="naam">De naam van de speler.</param>
        /// <returns>De niuwe speler.</returns>

        public GameOX FindSpelerByNaam(string naam)
        {
            foreach (GameOX speler in this._spelers)
            {
                if (speler.Naam == naam)
                {
                    return speler;
                }
            }

            return null;
        }
    }
}