namespace Slmm.Domain
{
    using System;

    public class Position: ICloneable
    {
        public Position(Coordinates coordinates, Orientation orientation)
        {
            this.Coordinates = coordinates;
            this.Orientation = orientation;
        }

        public Coordinates Coordinates { get; }
        public Orientation Orientation { get; }

        public object Clone()
        {
            return new Position(this.Coordinates.Clone() as Coordinates, this.Orientation);
        }
    }
}
