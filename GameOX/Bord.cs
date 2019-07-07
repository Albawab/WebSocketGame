// <copyright file="Bord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    ///
    /// </summary>
    public class Bord
    {
        private Teken[,] veldenOphetBord = null;
        private short _dimension = 0;
        private Spel _spel = null;

        public Bord(short dimension, Spel spel)
        {
            this._spel = spel;
            this.Dimension = dimension;
            this.veldenOphetBord = new Teken[dimension, dimension];
            this.ResetBord();
        }

        /// <summary>
        /// Gets wat is de grootte van de matrix.
        /// moet meer dan 2 zijn en kleiner dan 9.
        /// </summary>
        public short Dimension
        {
            get
            {
                return this._dimension;
            }

            private set
            {
                // todo
                if ((value < 2) && (value > 9))
                {
                    Console.WriteLine("Je mag alleen nummers tussen 2 en 9 invoeren");
                }

                this._dimension = value;
            }
        }

        /// <summary>
        /// Deze method tekent het bord er staat de teken op.
        /// </summary>
        /// <param name="speler">De naam van de speler</param>
        /// <param name="teken">welke teken de speler gaat gebruiken.</param>
        public void TekenBord()
        {
            // herschrijven , dat je de 1-- 9 index toont als het veld leeg is.
            short index = 1;
            StringBuilder line = new StringBuilder();
            string lijn = "";
            for (int column = 0; column < this.Dimension; column++)
            {
                lijn = "";
                for (int row = 0; row < this.Dimension; row++)
                {
                    if (this.veldenOphetBord[column, row] == Teken.Undefined)
                    {
                        if (row == 0)
                        {
                            line.Append("   ");
                        }

                        string indexToString = index.ToString();
                        if (index < 10)
                        {
                            indexToString = " " + index.ToString();
                        }

                        // toon de index in het scherm
                        line.AppendFormat("  {0}  ", indexToString);
                    }
                    else
                    {
                        // toon het teken in het scherm
                        if (row == 0)
                        {
                            line.Append("   ");
                        }

                        line.AppendFormat("   {0}  ", this.veldenOphetBord[column, row].ToString());
                    }

                    // + toevoegen of niet?
                    if (row != this.Dimension - 1)
                    {
                        line.Append("  |   ");
                        if (column != this.Dimension - 1)
                        {
                            lijn += "-----------+";
                        }
                    }

                    index++;
                }

                if (column != this.Dimension - 1)
                {
                    lijn += "-----------";
                }

                line.AppendLine();
                line.Append(lijn);
                line.AppendLine();
            }

            Console.WriteLine(line.ToString());

            // Actie actie = new Actie();
            // actie.StartDeSpel(speler, this._dimension, teken);
        }

        /// <summary>
        /// maak het bord leeg door de array met Teken.Undefined te vullen.
        /// </summary>
        public void ResetBord()
        {
            for (short column = 0; column < this.Dimension; column++)
            {
                for (short row = 0; row < this.Dimension; row++)
                {
                    this.ResetVeld(column, row);
                }
            }
        }

        /// <summary>
        /// reset het veld naar unddefied.
        /// </summary>
        /// <param name="index">index van het veld.</param>
        public void ResetVeld(short index)
        {
            short col, row = 0;
            this.ConvertIndexToArray(index, out col, out row);
            this.ResetVeld(col, row);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="column"></param>
        /// <param name="row"></param>
        public void ResetVeld(short column, short row)
        {
            this.veldenOphetBord[column, row] = Teken.Undefined;
        }

        /// <summary>
        /// bepaal of de gevraagd zet wel kan.
        /// </summary>
        /// <param name="indexOpHetBord">de gevraagde index van een zet.</param>
        /// <returns>true als de zet gedaan mag worden.</returns>
        public bool IsValidZet(short indexOpHetBord)
        {
            // todo Abdul bepalen
            // een valide zet, valt binnen het bord en het veld moet leeg zijn.
            // index > 0
            // index < (dimension * dimension)
            // [col, row] == Teken.Undefined
            short column = 0;
            short row = 0;
            this.ConvertIndexToArray(indexOpHetBord, out column, out row);
            if (this.veldenOphetBord[column, row] != Teken.Undefined)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="speler"></param>
        /// <param name="indexOpHetBord"></param>
        public void DoeZet(GameOX speler, short indexOpHetBord)
        {
            short column = 0;
            short row = 0;

            this.ConvertIndexToArray(indexOpHetBord, out column, out row);

            this.veldenOphetBord[column, row] = speler.TeGebruikenTeken;
        }

        public GameOX TegenSpeler(GameOX huidigeSpeler)
        {
            return this._spel.TegenSpeler(huidigeSpeler);
        }

        /// <summary>
        /// functie om te bepalen of het bord klaar is.
        /// </summary>
        /// <returns>true als het bord klaar is. </returns>
        public bool IsBordFinished()
        {
            // wanneer is het bord klaar?
            // als er een winnaar is;
            // of het bord is vol
            // de computer moet naar het bored kijken of het bord een cell leeg is.
            // bepaal voor elk veld op het bord of deze gevuld is?
            // Bepaal voor elke column/rij veld of het teken niet undefined is
            // loop door de kolommen en rijen en vbepaal van elk veld of het leeg is
            // zo ja, break dan ben je nog niet vol
            for (int column = 0; column < this.Dimension; column++)
            {
                for (int row = 0; row < this.Dimension; row++)
                {
                    if (this.veldenOphetBord[column, row] == Teken.Undefined)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// functie om uit te vragen of een teken gewonnen heeft.
        /// </summary>
        /// <param name="teken">het teken waarvan gekeken moet worden of het gewonnen heeft. </param>
        /// <returns>true als het teken gewonnen heeft. </returns>
        public bool HeeftTekenGewonnen(Teken teken)
        {
            bool heeftIemandGewonnen = false;

            // wanneer heeft een teken gewonnen?
            // horizontaal een hele rij
            // roep voor elke row op het bord de functie AreAllFieldsInTheRowEqual aan
            for (short rij = 0; rij < this.Dimension && !heeftIemandGewonnen; rij++)
            {
                heeftIemandGewonnen = this.AreAllFieldsInTheRowEqualTo(rij, teken);
                if (heeftIemandGewonnen)
                {
                    return true;
                }
            }

            // verticaal een hele rij
            for (short col = 0; col < this.Dimension && !heeftIemandGewonnen; col++)
            {
                heeftIemandGewonnen = this.AreAllFieldsInTheColEqualTo(col, teken);

                if (heeftIemandGewonnen)
                {
                    return true;
                }
            }

            // diagonaal een hele rij
            heeftIemandGewonnen = true;

            // van linksboven naar rechtsonder
            for (short colrow = 0; colrow < this.Dimension; colrow++)
            {
                if (this.veldenOphetBord[colrow, colrow] != teken)
                {
                    heeftIemandGewonnen = false;
                    break;
                }
            }

            if (heeftIemandGewonnen)
            {
                return heeftIemandGewonnen;
            }

            // van rechtsboven naar linksonder
            short maxDim = this.Dimension;
            maxDim--;

            for (short col = maxDim; col >= 0; col--)
            {
                for (short row = 0; row < this.Dimension; row++)
                {
                    if (this.veldenOphetBord[col, row] != teken)
                    {
                        heeftIemandGewonnen = false;
                        break;
                    }
                }
            }

            return heeftIemandGewonnen;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rij"></param>
        /// <param name="teken"></param>
        /// <returns></returns>
        private bool AreAllFieldsInTheRowEqualTo(short rij, Teken teken)
        {
            // loop door de kolomnen als er een teken anders is dan het gevraagde teken, return false
            for (short col = 0; col < this.Dimension; col++)
            {
                if (this.veldenOphetBord[col, rij] != teken)
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreAllFieldsInTheColEqualTo(short col, Teken teken)
        {
            // loop door de rijen als er een teken anders is dan het gevraagde teken, return false
            for (short rij = 0; rij < this.Dimension; rij++)
            {
                if (this.veldenOphetBord[col, rij] != teken)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// converteer de index naar columnen en rows.
        /// </summary>
        /// <param name="index">de te converteren index.</param>
        /// <param name="column">de column van de index.</param>
        /// <param name="row">de row van de index.</param>
        private void ConvertIndexToArray(short index, out short column, out short row)
        {
            column = 0;
            row = 0;

            // todo convert de 1--9 index to columns and rows which are 0 based;
            // schrijf hier op wat je wilt
            // (index - 1) / Dimension = column;
            // (index - 1) - (Dimension * column) = row;
            short zeroBasedIndex = --index;
            column = (short)(zeroBasedIndex / this.Dimension);
            row = (short)(zeroBasedIndex % this.Dimension);

            // iets doen met errorchecking?
        }

        /// <summary>
        /// convert van de col en row naar de index.
        /// </summary>
        /// <param name="column">column.</param>
        /// <param name="row">row. </param>
        /// <returns>index van de col en row.</returns>
        private short ConvertColRowToIndexToArray(short column, short row)
        {
            // gezeik
            short index = (short)(column * this.Dimension);

            // voeg de row toe
            index += row;

            // 1 toeveoegn omdat we oneBased zijn
            index++;

            // hoe kom ik van row en col naar index?
            return index;
        }

        /// <summary>
        /// geeft de mogelijke vrije velden terug op basis van index
        /// </summary>
        /// <returns>lijst met de mogelijke indexen</returns>
        public List<short> VrijVelden()
        {
            List<short> result = new List<short>();

            for (short col = 0; col < this.Dimension; col++)
            {
                for (short row = 0; row < this.Dimension; row++)
                {
                    if (this.veldenOphetBord[col, row] == Teken.Undefined)
                    {
                        result.Add(this.ConvertColRowToIndexToArray(col, row));
                    }
                }
            }

            return result;
        }
    }
}
