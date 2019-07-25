namespace HenE.Abdul.GameOX
{
    using System.Net.Sockets;
    using HenE.Abdul.Game_OX;
    using HenE.WebSocketExample.Shared.Infrastructure;

    public abstract class Speler : WebsocketBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Speler"/> class.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="dimention"></param>
        public Speler(string naam, short dimention)
        {
            this.Dimention = dimention;
            this.Naam = naam;
        }

        public string Naam { get; set; }

        public TcpClient TcpClient { get; set; }

        public short Dimention { get; set; }

        public Teken TeGebruikenTeken { get; set; }

        public void DoeZet()
        {
        }

        protected override string ProcessStream(string stream, TcpClient client)
        {
            string returnMessage = null;
            this.ProcessReturnMessage(stream, client);
            return returnMessage;
        }

        /// <summary>
        /// afvangen van het event dat het spel is gestart.
        /// </summary>
        public virtual void SpelStartedHandler()
        {
        }
    }
}
