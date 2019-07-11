using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HenE.Abdul.GameOX
{
    public abstract class Speler
    {
        public Speler(string naam,short dimention)
        {
            this.Dimention = dimention;
            this.Naam = naam;
        }

        public string Naam { get; private set; }

        public TcpClient tcpClient { get; set; }

        public short Dimention { get; set; }


        public void DoeZet()
        {

        }

        /// <summary>
        /// afvangen van het event dat het spel is gestart
        /// </summary>
        public virtual void SpelStartedHandler()
        {

        }
    }
}
