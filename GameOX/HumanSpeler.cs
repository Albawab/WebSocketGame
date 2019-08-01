// <copyright file="HumanSpeler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    /// <summary>
    /// De class van de human Speler.
    /// </summary>
    public class HumanSpeler : Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HumanSpeler"/> class.
        /// </summary>
        /// <param name="naam">De naam van de speler.</param>
        /// <param name="dimension"> De dimension.</param>
        public HumanSpeler(string naam, short dimension)
            : base(naam, dimension)
        {
        }

        /// <summary>
        /// De spelr doet een zet.
        /// </summary>
        /// <param name="nummer">Het nummer die de speler heeft gekozen.</param>
        /// <param name="gameOX">Het Spel.</param>
        /// <param name="bord">Het bord.</param>
        public override void SpelStartedHandler(short nummer, GameOX gameOX, Bord bord)
        {
              this.Zet(nummer, bord, gameOX);
        }
    }
}
