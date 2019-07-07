namespace HenE.WebSocketExample.Shared.Protocol
{
    /// <summary>
    /// met coommandos geet de client de server een opdracht
    /// </summary>
    public enum Commandos
    {
        NotDefined = 0,
        VerzoekTotDeelnemenSpel,
        StartSpel,
        WachtenOpAndereDeelnemer,
        DoeZet,
        ReStartSpel,
        VerlaatSpel,

    }
}
