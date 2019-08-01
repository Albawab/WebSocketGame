// <copyright file="RemoteHumanSpeler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX
{
    /// <summary>
    /// De calss van de remote human speler.
    /// </summary>
    public class RemoteHumanSpeler : HumanSpeler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteHumanSpeler"/> class.
        /// Remote human speler.
        /// </summary>
        /// <param name="naam">De naam.</param>
        /// <param name="dimension">De dimension.</param>
        public RemoteHumanSpeler(string naam, short dimension)
            : base(naam, dimension)
        {
            this.Dimension = dimension;
        }
    }
}
