namespace HenE.WebSocketExample.Shared.Protocol
{
    /// <summary>
    /// events gaan van de server naar de client
    /// </summary>
    public enum Events
    {
        NotDefined = 0,
        SpelGestart,
        WachtenOpAndereDeelnemer,
        YourTurn,
        SpelFinished,
        Bericht,
        Error = 99

    }
}
