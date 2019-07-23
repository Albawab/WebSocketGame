namespace HenE.Abdul.GameOX
{
    public class RemoteHumanSpeler : HumanSpeler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoteHumanSpeler"/> class.
        /// </summary>
        /// <param name="naam"></param>
        /// <param name="dimension"></param>
        public RemoteHumanSpeler(string naam, short dimension) : base(naam, dimension)
        {
            this.Dimention = dimension;
        }
    }
}
