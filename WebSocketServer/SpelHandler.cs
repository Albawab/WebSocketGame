// <copyright file="SpelHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace HenE.WebSocketExample.WebSocketServer
{
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Linq;
    using HenE.Abdul.GameOX;
    using HenE.WebSocketExample.Shared.Protocol;

    /// <summary>
    ///  class om alle spelhandelingen af te vangen.
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

        /// <summary>
        /// Vind een speler die het zelfde dimension heeft.
        /// </summary>
        /// <param name="dimension">dimension.</param>
        /// <returns>game.</returns>
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
        /// Create nieuw game.
        /// </summary>
        /// <param name="dimension">dimension.</param>
        /// <param name="player">player.</param>
        /// <param name="tcpClient">tcpClient.</param>
        /// <returns>nieuw game.</returns>
        public GameOX CreateGame(short dimension, string player, TcpClient tcpClient)
        {
            GameOX gameOX = new GameOX(dimension);

            this.currentSpellen.Add(gameOX);
            gameOX.AddPlayer(player, tcpClient, dimension);
            gameOX.AddBord(dimension);

            return gameOX;
        }

        /// <summary>
        /// Tegen de huidige speler.
        /// </summary>
        /// <param name="client">deze cleint.</param>
        /// <param name="tcps">List of clients.</param>
        /// <param name="tegenTcp">de tegenaar.</param>
        /// <returns>Event Wachat als string.</returns>
        public string TegeHuidigeClient(TcpClient client, List<TcpClient> tcps, out TcpClient tegenTcp)
        {
            tegenTcp = null;
            string returnMessage = string.Empty;
            foreach (TcpClient tcp in tcps)
            {
                if (tcp != client)
                {
                    tegenTcp = tcp;
                    returnMessage = EventHelper.CreateWachtenOpEenAndereDeelnemenEvent();
                }
            }

            return returnMessage;
        }

        public void CheckSpellen()
        {
            this.currentSpellen.RemoveAll(a => a.Status == GameOXStatussen.Finished);
        }

        /// <summary>
        /// /Get de speler from het spel.
        /// </summary>
        /// <param name="client">Huidig client.</param>
        /// <returns></returns>

        public Speler GetSpelerFromTcpClient(TcpClient client)
        {
            foreach (GameOX game in this.currentSpellen)
            {
                foreach (Speler spelr in game.Spelers)
                {
                    if (spelr.TcpClient == client)
                    {
                        return spelr;
                    }
                }
            }
            return null;
        }

        public void Remove(GameOX game)
        {
            this.currentSpellen.Remove(game);
        }
        public GameOX GetGameFromTcpClient(TcpClient tcpClient)
        {
            foreach (GameOX game in this.currentSpellen)
            {
                foreach (TcpClient tcp in game.TcpClients)
                {
                    if (tcp == tcpClient)
                    {
                        return game;
                    }
                }

            }
            return null;

        }


    }
}
