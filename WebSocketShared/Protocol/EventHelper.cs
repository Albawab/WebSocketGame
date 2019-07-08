namespace HenE.WebSocketExample.Shared.Protocol
{
    using HenE.Abdul.GameOX;
    using System;
    using System.Text;

    static public class EventHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        static public String CreateSpelgestartEvent(GameOX game)
        {
            // wat ga ik terug geven?
            // het commando en de lijste met spelers die meedoen, & gescheiden
            StringBuilder spelersnamen = new StringBuilder();
            foreach (var speler in game.Spelers)
            {
                if (spelersnamen.Length == 0)
                    spelersnamen.AppendFormat("{0}", speler.Naam);
                else
                    spelersnamen.AppendFormat("&{0}", speler.Naam);
            }

            return String.Format("{0}{1}", CreateEvent(Events.SpelGestart), spelersnamen.ToString());
        }

        static public Events CreateEenEvernt(string events)
        {
            Events e;
            if (Enum.TryParse(events, true, out e))
            {
                if (Enum.IsDefined(typeof(Events), events))
                {
                    return e;
                }
            }
            return Events.NotDefined;
        }

        /// <summary>
        /// Deze Method Created een Wacht Lijst van die speler die open zijn
        /// </summary>
        /// <returns></returns>
        static public String CreateWachtenOpEenAndereDeelnemenCommando()
        {
            // wat ga ik terug geven?
            // alleen het commando, rest hoeft niet

            return CreateEvent(Events.WachtenOpAndereDeelnemer);
        }

        static private String CreateEvent(Events e)
        {

            switch (e)
            {
                case Events.SpelGestart:
                    return "SpelGestart#";
                case Events.WachtenOpAndereDeelnemer:
                    return "WachtenOpAndereDeelnemer#";
            default:
                    throw new NotImplementedException();
            }
        }

        public static string CreateErrorEvent(Exception exp)
        {
            return String.Format("{0}{1}", CreateEvent(Events.Error), exp.Message);
        }
    }
}
