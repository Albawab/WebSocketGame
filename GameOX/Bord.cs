namespace HenE.Abdul.GameOX
{
    using System.Text;

    class Bord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bord"/> class.
        /// </summary>
        /// <param name="dimension"></param>
        public Bord(short dimension)
        {
            this.Dimension = dimension;
        }

        public short Dimension { get; set; }

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
                    if (row == 0)
                    {
                        line.Append("   ");
                    }

                    string indexToString = index.ToString();
                    if (index < 10)
                    {
                        indexToString = " " + index.ToString();
                        line.AppendFormat("  {0}  ", indexToString);
                    }

                    // toon de index in het scherm
                    else
                    {
                        // toon het teken in het scherm
                        if (row == 0)
                        {
                            line.Append("   ");
                        }

                        line.AppendFormat("      ");
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

            // Actie actie = new Actie();
            // actie.StartDeSpel(speler, this._dimension, teken);
        }
    }
}
