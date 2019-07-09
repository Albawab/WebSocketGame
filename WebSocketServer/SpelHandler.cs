using HenE.Abdul.GameOX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HenE.WebSocketExample.WebSocketServer
{

    /// <summary>
    ///  class om alle spelhandleingen af te vangen
    /// </summary>
    public class SpelHandler
    {
        private List<GameOX> currentSpellen = new List<GameOX>();

        public SpelHandler()
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimension"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="firstPlayer"></param>
        /// <returns></returns>
        public GameOX CreateGame(short dimension, string firstPlayer, TcpClient tcpClient)
        {
            GameOX gameOX = new GameOX(dimension);

            this.currentSpellen.Add(gameOX);
            gameOX.AddPlayer(firstPlayer, tcpClient,dimension);
            
            return gameOX;
        }


    }
}
