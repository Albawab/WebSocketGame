// <copyright file="TekenHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.Abdul.GameOX.Protocol
{
    using System;
    using System.Net.Sockets;
    using HenE.Abdul.Game_OX;

    /// <summary>
    /// De class die de class teken helpt.
    /// </summary>
    public static class TekenHelper
    {
        /// <summary>
        /// Omzetten de string tot event.
        /// </summary>
        /// <param name="teken">Teken als string.</param>
        /// <returns>Teken als event.</returns>
        public static Teken CreateTekenEnum(string teken)
        {
            if (Enum.TryParse(teken, true, out Teken result))
            {
                return result;
            }

            return result;
        }

        /// <summary>
        /// Geef een teken aan de speler.
        /// </summary>
        /// <param name="teken">Teken.</param>
        /// <param name="oX">De list of de Games.</param>
        /// <param name="client">Huidige client.</param>
        public static void AddTekenToSpeler(Teken teken, GameOX game, TcpClient client)
        {
            Teken huidigeTeken = teken;
            foreach (Speler speler in game.Spelers)
            {
                if (speler.TcpClient == client)
                {
                    speler.TeGebruikenTeken = teken;
                }
                else
                {
                    speler.TeGebruikenTeken = TegenHuidigeTeken(huidigeTeken);
                }
            }

        }

        private static Teken TegenHuidigeTeken(Teken huidigeteken)
        {
            if (huidigeteken == Teken.O)
            {
                return Teken.X;
            }
            else
            {
                return Teken.O;
            }
        }
    }
}
