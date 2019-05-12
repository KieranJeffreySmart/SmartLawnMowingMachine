namespace Slmm.Domain
{
    public class Coordinates
    {
        public Coordinates(int xCoordinate, int yCoordinate)
        {
            this.X = xCoordinate;
            this.Y = yCoordinate;
        }

        public int X { get; }
        public int Y { get; }
    }
}
