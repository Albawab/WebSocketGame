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
        public Speler(string naam,short dim)
        {
            this.dimention = dimention;
            this.Naam = naam;
        }


        public string Naam { get; private set; }

        public TcpClient tcpClient { get; set; }

        public short dimention { get; set; }
        
    }
}
