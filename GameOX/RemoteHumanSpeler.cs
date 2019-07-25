namespace HenE.Abdul.GameOX
{
    public class RemoteHumanSpeler : HumanSpeler
    {
        public RemoteHumanSpeler(string naam, short dimension) : base(naam, dimension)
        {
            this.Dimention = dimension;
        }
    }
}
