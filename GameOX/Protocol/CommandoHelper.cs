﻿namespace HenE.WebSocketExample.Shared.Protocol
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
        ///
        /// </summary>
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
        /// ik krijg binnen jos&3 binnen.
        /// </summary>
        /// <param name="commandParams">het param gedeelte van de message.</param>
        /// <param name="spelersnaam">naam van de speler.</param>
        /// <param name="dimension">dimension.</param>
        /// <returns></returns>
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
        /// <returns></returns>
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
                default:
                    throw new NotImplementedException();
            }
        }
    }
}