// <copyright file="Bord.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using HenE.Abdul.Game_OX;

    /// <summary>
    /// De class van het bord.
    /// </summary>
    public class Bord
    {
        private readonly Teken[,] veldenOphetBord = null;
        private readonly List<Teken[,]> tekens = new List<Teken[,]>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Bord"/> class.
        /// </summary>
        /// <param name="dimension">Dimension.</param>
        public Bord(short dimension)
        {
            this.Dimension = dimension;
            this.veldenOphetBord = new Teken[dimension, dimension];
        }

        /// <summary>
        /// Gets or sets de dimension .
        /// </summary>
        public short Dimension { get; set; }

        /// <summary>
        /// Deze method teken het bord.
        /// </summary>
        /// <returns>Het bord als text.</returns>
        public string TekenBord()
        {
            // herschrijven , dat je de 1-- 9 index toont als het veld leeg is.
            short index = 1;
            StringBuilder line = new StringBuilder();
            string lijn = string.Empty;
            for (int column = 0; column < this.Dimension; column++)
            {
                lijn = string.Empty;
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

            return line.ToString();
        }

        /// <summary>
        /// Check of het bord vol is.
        /// </summary>
        /// <returns>true of false.</returns>
        public bool IsBordFinished()
        {
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
        /// Zet een teken op het bord.
        /// </summary>
        /// <param name="speler">Speler.</param>
        /// <param name="indexOpHetBord">Index op het bord.</param>
        public void DoeZet(Speler speler, short indexOpHetBord)
        {
            this.ConvertIndexToArray(indexOpHetBord, out short column, out short row);

            this.veldenOphetBord[column, row] = speler.TeGebruikenTeken;
        }

        /// <summary>
        /// bepaal of de gevraagd zet wel kan.
        /// </summary>
        /// <param name="indexOpHetBord">de gevraagde index van een zet.</param>
        /// <returns>true als de zet gedaan mag worden.</returns>
        public bool IsValidZet(short indexOpHetBord)
        {
            this.ConvertIndexToArray(indexOpHetBord, out short column, out short row);
            if (this.veldenOphetBord[column, row] != Teken.Undefined)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// geeft de mogelijke vrije velden terug op basis van index.
        /// </summary>
        /// <returns>lijst met de mogelijke indexen.</returns>
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

            heeftIemandGewonnen = true;

            for (short row = 0; row < this.Dimension; row++)
            {
                if (this.veldenOphetBord[maxDim--, row] != teken)
                {
                    heeftIemandGewonnen = false;
                    break;
                }
            }

            return heeftIemandGewonnen;
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
        /// Als dit cel bezit is, dan laat maar de spel dat weten .
        /// </summary>
        /// <param name="tcp">Huidige client.</param>
        /// <param name="game">huidige game.</param>
        public void HetBordIsVol(TcpClient tcp, GameOX game)
        {
            game.HetIsBezit(tcp);
        }

        /// <summary>
        /// Doet deze method het bord leeg.
        /// </summary>
        /// <param name="column">column.</param>
        /// <param name="row">row.</param>
        public void ResetVeld(short column, short row)
        {
            this.veldenOphetBord[column, row] = Teken.Undefined;
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
            short zeroBasedIndex = --index;
            column = (short)(zeroBasedIndex / this.Dimension);
            row = (short)(zeroBasedIndex % this.Dimension);
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
        /// controleer of de row heeft het zelfde teken.
        /// </summary>
        /// <param name="rij">rij.</param>
        /// <param name="teken">teken.</param>
        /// <returns>False of true.</returns>
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
    }
}
