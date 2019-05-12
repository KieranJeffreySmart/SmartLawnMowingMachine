namespace Slmm.Domain
{
    public class Position
    {
        public Position(Coordinates coordinates, Orientation orientation)
        {
            this.Coordinates = coordinates;
            this.Orientation = orientation;
        }

        public Coordinates Coordinates { get; }
        public Orientation Orientation { get; }
    }
}
