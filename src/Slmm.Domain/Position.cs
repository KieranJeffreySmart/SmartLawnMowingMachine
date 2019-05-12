namespace Slmm.Domain
{
    public class Position
    {
        private readonly Coordinates coordinates;
        private readonly Orientation orientation;

        public Position(Coordinates coordinates, Orientation orientation)
        {
            this.coordinates = coordinates;
            this.orientation = orientation;
        }
    }
}
