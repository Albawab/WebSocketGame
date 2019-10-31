// <copyright file="HumSpeler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.VierOPEenRij
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Gaat over de Human speler.
    /// </summary>
    internal class HumSpeler : Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HumSpeler"/> class.
        /// </summary>
        /// <param name="naam">De naam van een speler.</param>
        public HumSpeler(string naam)
            : base(naam)
        {
        }

        /// <summary>
        /// Gets or sets de naam van de speler.
        /// </summary>
        public string Naam { get; set; }
    }
}
