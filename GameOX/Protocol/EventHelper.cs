// <copyright file="EventHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.Shared.Protocol
{
    using System;
    using System.Text;
    using HenE.Abdul.GameOX;

    /// <summary>
    /// Helper de events.
    /// </summary>
    public static class EventHelper
    {
        /// <summary>
        /// terug geven als de spel is gestart.
        /// </summary>
        /// <param name="game">huidige game.</param>
        /// <returns>De message die naar de client gaat als string. De namen van de spelers en de dimension.</returns>
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
                    spelersnamen.AppendFormat("&{0}", speler.Dimension);
                }
            }

            return string.Format("{0}{1}", CreateEvent(Events.SpelGestart), spelersnamen.ToString());
        }

        /// <summary>
        /// De event komt naar hier toe als string .
        /// Deze method gaat de string omzetten.
        /// </summary>
        /// <param name="events">De event als string.</param>
        /// <returns>De Event.</returns>
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
        /// <returns>De Event als string.</returns>
        public static string CreateWachtenOpEenAndereDeelnemenEvent()
        {
            return CreateEvent(Events.WachtenOpAndereDeelnemer);
        }

        /// <summary>
        /// Omzetten de even tot string.
        /// </summary>
        /// <param name="event">De event.</param>
        /// <returns>Event als string.</returns>
        public static string CreateEvents(Events @event)
        {
            return CreateEvent(@event);
        }

        /// <summary>
        /// Als er een error is.
        /// </summary>
        /// <param name="exp">Exception.</param>
        /// <returns>Event als string.</returns>
        public static string CreateErrorEvent(Exception exp)
        {
            return string.Format("{0}{1}", CreateEvent(Events.Error), exp.Message);
        }

        /// <summary>
        /// Verdeelt de message die vanuit de speler komt .
        /// De eerste deel is de Protocol als string.
        /// </summary>
        /// <param name="mess">De message.</param>
        /// <param name="eventsParams">De rest van de message.</param>
        /// <returns>Event.</returns>
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
                case Events.YourTurn:
                    return "YourTurn#";
                case Events.Winnaar:
                    return "Winnaar#";
                case Events.NieuwRondje:
                    return "NieuwRondje#";
                case Events.StartNieuwRond:
                    return "StartNieuwRond#";
                case Events.IsGewonnen:
                    return "IsGewonnen#";
                case Events.HetIsBezit:
                    return "HetIsBezit#";
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
