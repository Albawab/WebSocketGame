// <copyright file="ComputerSpeler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    using System.Collections.Generic;

    /// <summary>
    /// De class van de computer speler.
    /// </summary>
    public class ComputerSpeler : Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComputerSpeler"/> class.
        /// </summary>
        /// <param name="naam">De naam van de computer speler.</param>
        /// <param name="dimension">Dimension.</param>
        public ComputerSpeler(string naam, short dimension)
            : base(naam, dimension)
        {
        }

        /// <inheritdoc/>
        public override void SpelStartedHandler(short nummer, GameOX gameOX, Bord bord)
        {
            bool magZet = true;
            List<short> vrijeVelden = bord.VrijVelden();

            foreach (short index in vrijeVelden)
            {
                bord.DoeZet(this, index);

                // als ik mijn teken daar invul, win ik dan.
                // of wint de tegenstander dan?
                if (bord.HeeftTekenGewonnen(this.TeGebruikenTeken))
                {
                    bord.ResetVeld(index);
                    this.Zet(index, bord, gameOX);
                    magZet = false;
                    break;
                }
                else
                {
                    Speler tegenSpeler = gameOX.TegenSpeler(this);

                    bord.DoeZet(tegenSpeler, index);
                    if (bord.HeeftTekenGewonnen(tegenSpeler.TeGebruikenTeken))
                    {
                        // tegenstander heeft gewonnen
                        bord.ResetVeld(index);
                        this.Zet(index, bord, gameOX);
                        magZet = false;
                        break;
                    }
                    else
                    {
                        bord.ResetVeld(index);
                    }
                }
            }

            if (magZet)
            {
                this.Zet(vrijeVelden[0], bord, gameOX);
            }
        }
    }
}
