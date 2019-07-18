using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HenE.Abdul.GameOX
{
    /// <summary>
    /// 
    /// </summary>
    public class HumanSpeler : Speler
    {
        public HumanSpeler(string naam,short dimention) : base(naam , dimention)
        {
        }

        public override void SpelStartedHandler()
        {
        }
    }
}
