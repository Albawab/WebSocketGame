// <copyright file="Speler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    using System.Net.Sockets;
    using HenE.Abdul.Game_OX;

    /// <summary>
    /// De class van de speler.
    /// </summary>
    public abstract class Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Speler"/> class.
        /// </summary>
        /// <param name="naam">De naam van de speler.</param>
        /// <param name="dimension">De dimenion van het bord.</param>
        protected Speler(string naam, short dimension)
        {
            this.Dimension = dimension;
            this.Naam = naam;
        }

        /// <summary>
        /// Gets or sets de namen van de spelers.
        /// </summary>
        public string Naam { get; set; }

        /// <summary>
        /// Gets or sets de clients van de spelrs.
        /// </summary>
        public TcpClient TcpClient { get; set; }

        /// <summary>
        /// Gets or sets de dimention van het bord.
        /// </summary>
        public short Dimension { get; set; }

        /// <summary>
        /// Gets or sets de teken van de speler.
        /// </summary>
        public Teken TeGebruikenTeken { get; set; }

        /// <summary>
        /// Gets de punten.
        /// </summary>
        public int Punten { get; private set; }

        /// <summary>
        /// Doe een zet op het bord.
        /// </summary>
        /// <param name="nummer">Het nummer die de speler heeft gekozen.</param>
        /// <param name="huidigBord">Huidig bord.</param>
        /// <param name="game">Huidig spel.</param>
        public void Zet(short nummer, Bord huidigBord, GameOX game)
        {
            Bord bord = huidigBord;

            string tekenBords = string.Empty;
            short indexOpHetBord = nummer;

            bord.DoeZet(this, indexOpHetBord);
        }

        /// <summary>
        /// Dit method geef een punt aan de winnaar.
        /// </summary>
        public void BeeindigBord()
        {
            this.Punten++;
        }

        /// <summary>
        /// afvangen van het event dat het spel is gestart.
        /// </summary>
        /// <param name="nummer">Het nummer die de speler heeft gekozen.</param>
        /// <param name="gameOX">Het spel.</param>
        /// <param name="bord">Het bord.</param>
        public virtual void SpelStartedHandler(short nummer, GameOX gameOX, Bord bord)
        {
        }
    }
}
