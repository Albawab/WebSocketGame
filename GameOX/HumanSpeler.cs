using System;
using System.Threading.Tasks;

namespace HenE.Abdul.GameOX
{
    /// <summary>
    ///
    /// </summary>
    public class HumanSpeler : Speler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HumanSpeler"/> class.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="dimention"></param>
        public HumanSpeler(string naam, short dimention) : base(naam, dimention)
        {
        }

        public override void SpelStartedHandler()
        {
            short nummer = 0;

            this.ProcessStream("SpelerGestart#", this.TcpClient);
        }
    }
}
