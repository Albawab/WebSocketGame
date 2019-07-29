// <copyright file="TekenHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using HenE.Abdul.Game_OX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HenE.Abdul.GameOX.Protocol
{
    public static class TekenHelper
    {
        public static Teken CreateTekenEnum(string teken)
        {
            Teken result;
            if (Enum.TryParse(teken, true, out result))
            {
                return result;
            }

            return result;
        }

        public static void AddTekenToSpeler(Teken teken, List<GameOX> oX, TcpClient client)
        {
            Teken huidigeTeken = teken;
            foreach (GameOX ox in oX)
            {
                foreach (Speler speler in ox.Spelers)
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
