namespace Slmm.Domain
{
    public class Mower
    {
        private Garden garden;
        private Position position;

        public Mower(Garden garden)
        {
            this.garden = garden;
        }

        public bool Start(Position startingPosition)
        {
            return true;
        }
    }
}
