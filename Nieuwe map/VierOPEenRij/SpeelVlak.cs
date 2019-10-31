// <copyright file="SpeelVlak.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.VierOPEenRij
{
    using System.Text;
    using HenE.VierOPEenRij.Enum;

    /// <summary>
    /// Klas om speelvlak te teken.
    /// Reset het speelvlak.
    /// </summary>
    internal class SpeelVlak
    {
        private readonly Teken[,] velderInHetSpeelvlak = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpeelVlak"/> class.
        /// </summary>
        /// <param name="dimension">De grootte van het speelvak.</param>
        public SpeelVlak(int dimension)
        {
            this.velderInHetSpeelvlak = new Teken[dimension, dimension];
        }

        /// <summary>
        /// Teken het speelvlak.
        /// </summary>
        /// <param name="dimension">Hoe is het speelvlak groot is.</param>
        /// <returns>de speelvlak.</returns>
        public string TekenSpeelvlak(int dimension)
        {
            StringBuilder teken = new StringBuilder();
            int columnNummer = 0;
            for (int rij = 0; rij < dimension; rij++)
            {
                for (int column = 0; column < dimension - 1; column++)
                {
                    if (columnNummer == 0)
                    {
                        teken.Append($"   {column}   ");
                    }
                    else
                    {
                        if (this.velderInHetSpeelvlak[rij, column] == Teken.Indefinite)
                        {
                            teken.Append("----|----");
                            column++;
                        }
                        else
                        {
                            teken.Append($"   {this.velderInHetSpeelvlak[rij, column].ToString()}   ");
                        }
                    }
                }

                teken.AppendLine();
                teken.AppendLine();
            }

            return teken.ToString();
        }

        /// <summary>
        /// Reset the table.
        /// </summary>
        /// <param name="dimension">De grootte van het speelvlak waar de method loopt door.</param>
        public void ResetSpeelVlak(int dimension)
        {
            for (int rij = 0; rij < dimension; rij++)
            {
                for (int column = 0; column < dimension; column++)
                {
                    this.ResetVeld(rij, column);
                }
            }
        }

        /// <summary>
        /// Reset one cell.
        /// </summary>
        /// <param name="rij">Het nummer  van de rij.</param>
        /// <param name="column">Het nummer van de column.</param>
        public void ResetVeld(int rij, int column)
        {
            this.velderInHetSpeelvlak[rij, column] = Teken.Indefinite;
        }

        /// <summary>
        /// Check of de column nog een vrij veld heeft of niet.
        /// </summary>
        /// <param name="column">De nummer van de column waar de method door loopt.</param>
        /// <param name="dimension">De grootte van de column.</param>
        /// <returns>Heeft deze column een vrij veld of niet.</returns>
        public bool MagInzetten(int column, int dimension)
        {
            for (int index = 0; index < dimension; column++)
            {
                if (this.velderInHetSpeelvlak[index, column] == Teken.Indefinite)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check of het speelvlak vol of nog niet helemaal is.
        /// </summary>
        /// <param name="dimension">De grootte van het speelvlak.</param>
        /// <returns>Is het speelvlak vol of nog niet.</returns>
        public bool IsSpeelvlakVol(int dimension)
        {
            for (int rij = 0; rij < dimension; rij++)
            {
                for (int column = 0; column < dimension; column++)
                {
                    if (this.velderInHetSpeelvlak[rij, column] == Teken.Indefinite)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
