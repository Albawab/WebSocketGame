﻿namespace HenE.WebSocketExample.Shared.Protocol
{
    using System;
    using System.Text;
    using HenE.Abdul.GameOX;

    public static class EventHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public static string CreateSpelgestartEvent(GameOX game)
        {
            // wat ga ik terug geven?
            // het commando en de lijste met spelers die meedoen, & gescheiden
            StringBuilder spelersnamen = new StringBuilder();
            foreach (Speler speler in game.Spelers)
            {
                if (game.FindSpelerByNaam(speler) == speler.Naam)
                {
                    speler.Naam = speler.Naam + 1;
                }

                break;
            }

            foreach (var speler in game.Spelers)
            {
                if (spelersnamen.Length == 0)
                {
                    spelersnamen.AppendFormat("{0}", speler.Naam);
                }
                else
                {
                    spelersnamen.AppendFormat("&{0}", speler.Naam);
                    spelersnamen.AppendFormat("&{0}", speler.Dimention);
                }
            }

            return string.Format("{0}{1}", CreateEvent(Events.SpelGestart), spelersnamen.ToString());
        }

        public static Events CreateEenEvent(string events)
        {
            if (Enum.TryParse(events, true, out Events e))
            {
                if (Enum.IsDefined(typeof(Events), events))
                {
                    return e;
                }
            }

            return Events.NotDefined;
        }

        /// <summary>
        /// Deze Method Created een Wacht Lijst van die speler die open zijn.
        /// </summary>
        /// <returns></returns>
        public static string CreateWachtenOpEenAndereDeelnemenEvent()
        {
            // wat ga ik terug geven?
            // alleen het commando, rest hoeft niet
            return CreateEvent(Events.WachtenOpAndereDeelnemer);
        }

        public static string CreateSpelerGestartEvent()
        {
            return CreateEvent(Events.SpelerGestart);
        }

        private static string CreateEvent(Events e)
        {
            switch (e)
            {
                case Events.SpelGestart:
                    return "SpelGestart#";
                case Events.SpelerGestart:
                    return "SpelerGestart#";
                case Events.WachtenOpAndereDeelnemer:
                    return "WachtenOpAndereDeelnemer#";
                case Events.Bericht:
                    return "Bericht#";
                default:
                    throw new NotImplementedException();
            }
        }

        public static string CreateErrorEvent(Exception exp)
        {
            return string.Format("{0}{1}", CreateEvent(Events.Error), exp.Message);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mess"></param>
        /// <param name="eventsParams"></param>
        /// <returns></returns>
        public static Events SplitEventAndParamsFromMessage(string mess, out string eventsParams)
        {
            eventsParams = string.Empty;

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
                eventsParams = opgeknipt[1];
            }

            // probeer dan de eerste om te zetten naar een commandos
            Events result = Events.NotDefined;

            // Omzetten de string to enum
            if (Enum.TryParse(opgeknipt[0], true, out result))
            {
                if (Enum.IsDefined(typeof(Events), result))
                {
                    return result;
                }
            }

            // niet gevonden, dus info was fout
            throw new NotImplementedException();
        }
    }
}