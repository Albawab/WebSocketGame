// <copyright file="Speler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.VierOPEenRij
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Vraagt de speler om zet te doen.
    /// Vraag de speler om naam te geven.
    /// </summary>
    internal class Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Speler"/> class.
        /// </summary>
        /// <param name="naam">De naam van de speler.</param>
        public Speler(string naam)
        {
            this.Naam = naam;
        }

        /// <summary>
        /// Gets or sets de naam van de speler.
        /// </summary>
        private string Naam { get; set; }
    }
}
