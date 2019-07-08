using System.Collections.Generic;
using System.Net.Sockets;
using System;

namespace HenE.Abdul.GameOX
{
    /// <summary>
    /// todo Abdul, alles koprieren uit de spel class
    /// </summary>
    public class GameOX
    {
       
       public IList<GameOX> Spelers = new List<GameOX>();
       
        List<TcpClient> tcpClients = new List<TcpClient>();

        public GameOX(short dimension)
        {
            this.Status = GameOXStatussen.NogNietGestart;
        }
        public string Naam { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public GameOXStatussen Status { get; private set; }

        public short Dimension { get; set; }
        

        public bool IsWaitingForGamers()
        {
            return this.Status == GameOXStatussen.Waiting;
        }

        public void AddPlayer(string stream, TcpClient client) {
            Naam = stream;
            tcpClients.Add(client);
        }
    }
}
