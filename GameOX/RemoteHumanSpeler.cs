using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace HenE.Abdul.GameOX
{
    public class RemoteHumanSpeler : HumanSpeler
    {
        public RemoteHumanSpeler(string naam,short dimension, TcpClient tcpClient) : base(naam, dimension)
        {
        }
        

    }
}
