// <copyright file="Commandos.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.Shared.Protocol
{
    /// <summary>
    /// met coommandos geet de client de server een opdracht.
    /// </summary>
    public enum Commandos
    {
        /// <summary>
        /// Not yet Defind.
        /// </summary>
        NotDefined = 0,

        /// <summary>
        /// Zoek voor andre speler.
        /// </summary>
        VerzoekTotDeelnemenSpel,

        /// <summary>
        /// Het spel gaat starten.
        /// </summary>
        StartSpel,

        /// <summary>
        /// De speler is gestart.
        /// </summary>
        SpelerGestart,

        /// <summary>
        /// Wacht op de andre speler om reactie te geven.
        /// </summary>
        WachtenOpAndereDeelnemer,

        /// <summary>
        /// Zit op het bord.
        /// </summary>
        DoeZet,

        /// <summary>
        /// Replay.
        /// </summary>
        ReStartSpel,

        /// <summary>
        /// Als de speler een nieuw rondje wil doen.
        /// </summary>
        NieuwRondje,

        /// <summary>
        /// Als de speler het spel heeft verlaten.
        /// </summary>
        VerlaatSpel,

        /// <summary>
        /// Waneer het spel is klaar.
        /// </summary>
        BeeindigSpel,
    }
}
