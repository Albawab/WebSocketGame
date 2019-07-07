namespace HenE.WebSocketExample.Shared.Protocol
{
    /// <summary>
    /// events gaan van de server naar de client
    /// </summary>
    public enum Events
    {
        SpelGestart,
        WachtenOpAndereDeelnemer,
        YourTurn,
        SpelFinished,
        Error = 99

    }
}
