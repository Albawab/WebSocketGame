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
       
       public IList<Speler> Spelers = new List<Speler>();
               
        public GameOX(short dimension)
        {
            this.Dimension = dimension;
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

        public void AddPlayer(string naam, TcpClient client,short dimention)
        {
            Spelers.Add(new HumanSpeler(naam, dimention) { tcpClient = client });
        }

        public void WachtOpAndereSpeler()
        {
            this.Status = GameOXStatussen.Waiting;
        }
    }
}
