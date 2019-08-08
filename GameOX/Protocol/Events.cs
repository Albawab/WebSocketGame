// <copyright file="Events.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.Shared.Protocol
{
    /// <summary>
    /// events gaan van de server naar de client.
    /// </summary>
    public enum Events
    {
        /// <summary>
        /// Not yet defined.
        /// </summary>
        NotDefined = 0,

        /// <summary>
        /// De speler is gestart.
        /// </summary>
        SpelerGestart,

        /// <summary>
        /// Het spel gaat starten.
        /// </summary>
        SpelGestart,

        /// <summary>
        /// Wacht op de andre speler om reactie te geven.
        /// </summary>
        WachtenOpAndereDeelnemer,

        /// <summary>
        /// Hier start de andre speler.
        /// </summary>
        YourTurn,

        /// <summary>
        /// Wie is gewonnen.
        /// </summary>
        Winnaar,

        /// <summary>
        /// Als de speler een nieuw rondje wil doen.
        /// </summary>
        NieuwRondje,

        /// <summary>
        /// Start een nieuw rond.
        /// </summary>
        StartNieuwRond,

        /// <summary>
        /// Wie is gewonnen.
        /// </summary>
        IsGewonnen,

        /// <summary>
        /// Als het cel bezit is.
        /// </summary>
        HetIsBezit,

        /// <summary>
        /// Het bord is vol.
        /// </summary>
        BordIsVol,

        /// <summary>
        /// Als de speler wil bliven spelen.
        /// </summary>
        NieuwSpel,

        /// <summary>
        /// Als niemand heeft gewonnen.
        /// </summary>
        NiemandGewonnen,


        SpelGeannuleerd,


        /// <summary>
        /// Error.
        /// </summary>
        Error = 99,
    }
}
