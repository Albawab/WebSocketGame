// <copyright file="CommandoHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.Shared.Protocol
{
    using System;
    using System.Text;
    using HenE.Abdul.GameOX;

    /// <summary>
    /// class cvoor de helper functies van commandos.
    /// </summary>
    public static class CommandoHelper
    {
        /// <summary>
        /// maak een message voor het commando VerzoekTotDeelnemenSpel.
        /// </summary>
        /// <param name="spelersnaam"> naam van de speler.</param>
        /// <param name="dimension"> welke dimension wil de speler.</param>
        /// <returns>message welke verstuurd kan worden naar de server. </returns>
        public static string CreateVerzoekTotDeelnemenSpelCommando(string spelersnaam, short dimension)
        {
            return string.Format("{0}{1}&{2}", CreateCommando(Commandos.VerzoekTotDeelnemenSpel), spelersnaam, dimension);
        }

        /// <summary>
        /// Maak een message voor  Commando spelgestart.
        /// </summary>
        /// <param name="teken"> De teken die de speler wil gebruiken.</param>
        /// <returns> De message.</returns>
        public static string CreateSpelerGestartCommando(string teken)
        {
            return string.Format("{0}{1}", CreateCommando(Commandos.SpelerGestart), teken);
        }

        /// <summary>
        /// Maak een message voor  Commando Doezet.
        /// </summary>
        /// <param name="nummer">het nummer die de speler wil doen.</param>
        /// <returns>De message.</returns>
        public static string DoeZet(short nummer)
        {
            return string.Format("{0}{1}", CreateCommando(Commandos.DoeZet), nummer);
        }

        /// <summary>
        /// Maak een massage voor commando NieuwRondje.
        /// </summary>
        /// <param name="nieuwRondje">Als de speler ja of nee zegt.</param>
        /// <returns>De message.</returns>
        public static string NieuwRondje(string nieuwRondje)
        {
            return string.Format("{0}{1}", CreateCommando(Commandos.NieuwRondje), nieuwRondje);
        }

        /// <summary>
        /// Maak een message voor Commando BeeindigSpel.
        /// </summary>
        /// <returns>De message.</returns>
        public static string BeeindigSpel()
        {
            return string.Format("{0}", CreateCommando(Commandos.BeeindigSpel));
        }

        /// <summary>
        /// Als het spel is starten , Dan geven hier de naam met de dimension als string en het gaat naar de client.
        /// </summary>
        /// <param name="game">De game.</param>
        /// <returns>De message die naar de client gaat.</returns>
        public static string CreateStartGameCommando(GameOX game)
        {
            // wat ga ik terug geven?
            // het commando en de lijste met spelers die meedoen, & gescheiden
            StringBuilder spelersnamen = new StringBuilder();
            foreach (var speler in game.Spelers)
            {
                if (spelersnamen.Length == 0)
                {
                    spelersnamen.AppendFormat("{0}", speler.Naam);
                }
                else
                {
                    spelersnamen.AppendFormat("&{0}", speler.Naam);
                }
            }

            return string.Format("{0}{1}", CreateCommando(Commandos.StartSpel), spelersnamen.ToString());
        }

        /// <summary>
        /// knip de message op in een command en params.
        /// </summary>
        /// <param name="mess">de ontvangen message.</param>
        /// <param name="commandParams">het gedeelte na de #.</param>
        /// <returns>het gevonden commando.</returns>
        public static Commandos SplitCommandAndParamsFromMessage(string mess, out string commandParams)
        {
            commandParams = string.Empty;

            // Split de string voor # en na
            string[] opgeknipt = mess.Split(new char[] { '#' });

            // controleer of er wel een # aanwezig is
            if (opgeknipt.Length == 0)
            {
                throw new ArgumentOutOfRangeException("message bevat geen #.");
            }

            // wij hebben array met array[0] en array[1]
            // als het goorter is dan array[1] dan doe het maar bij array[1]
            if (opgeknipt.Length > 1)
            {
                commandParams = opgeknipt[1];
            }

            // probeer dan de eerste om te zetten naar een commandos
            Commandos result = Commandos.NotDefined;

            // Omzetten de string to enum
            if (Enum.TryParse(opgeknipt[0], true, out result))
            {
                if (Enum.IsDefined(typeof(Commandos), result))
                {
                    return result;
                }
            }

            // niet gevonden, dus info was fout
            throw new NotImplementedException();
        }

        /// <summary>
        /// ik krijg binnen de naam met de dimension binnen.
        /// </summary>
        /// <param name="commandParams">het param gedeelte van de message.</param>
        /// <param name="spelersnaam">naam van de speler.</param>
        /// <param name="dimension">dimension.</param>
        /// <returns>true.</returns>
        public static bool SplitVerzoektotDeelnemeSpelParameterFromMessage(string commandParams, out string spelersnaam, out short dimension)
        {
            spelersnaam = string.Empty;
            dimension = 0;
            return true;
        }

        /// <summary>
        /// maakt het standaard commandm
        /// afgesproken is om elk commando te beëindigen met een #.
        /// </summary>
        /// <param name="commando">enumerator van commando.</param>
        /// <returns>De commandos als string.</returns>
        private static string CreateCommando(Commandos commando)
        {
            switch (commando)
            {
                case Commandos.VerzoekTotDeelnemenSpel:
                    return "VerzoekTotDeelnemenSpel#";
                case Commandos.WachtenOpAndereDeelnemer:
                    return "WachtenOpAndereDeelnemer#";
                case Commandos.StartSpel:
                    return "StartSpel#";
                case Commandos.DoeZet:
                    return "DoeZet#";
                case Commandos.SpelerGestart:
                    return "SpelerGestart#";
                case Commandos.NieuwRondje:
                    return "nieuwRondje#";

                case Commandos.BeeindigSpel:
                    return "BeeindigSpel#";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
