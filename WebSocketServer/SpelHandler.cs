namespace HenE.WebSocketExample.WebSocketServer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;
    using HenE.Abdul.GameOX;

    /// <summary>
    ///  class om alle spelhandleingen af te vangen.
    /// </summary>
    public class SpelHandler
    {
        private readonly List<GameOX> currentSpellen = new List<GameOX>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SpelHandler"/> class.
        /// </summary>
        public SpelHandler()
        {
        }

        public GameOX GetOpenSpelbyDimension(short dimension)
        {
            foreach (GameOX gameOX in this.currentSpellen)
            {
                if (gameOX.IsWaitingForGamers() && gameOX.Dimension == dimension)
                {
                    return gameOX;
                }
            }

            return null;
        }

        public GameOX CreateGame(short dimension, string player, TcpClient tcpClient)
        {
            GameOX gameOX = new GameOX(dimension);

            this.currentSpellen.Add(gameOX);
            gameOX.AddPlayer(player, tcpClient, dimension);

            return gameOX;
        }
    }
}
