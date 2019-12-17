namespace slmm.LawnMowing.Model
{
    using System;

    public class Coordinates: ICloneable
    {
        public Coordinates(int xCoordinate, int yCoordinate)
        {
            this.X = xCoordinate;
            this.Y = yCoordinate;
        }

        public int X { get; }
        public int Y { get; }

        public object Clone()
        {
            return new Coordinates(this.X, this.Y);
        }
    }
}
