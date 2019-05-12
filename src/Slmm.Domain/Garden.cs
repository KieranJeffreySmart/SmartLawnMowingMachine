using System;

namespace Slmm.Domain
{

    public class Garden
    {
        private readonly int[,] map;

        public Garden(int length, int width)
        {
            this.map = new int[length,width];
        }

        internal bool CellIsEmpty(Coordinates coordinates)
        {
            return coordinates.X > 0 && coordinates.X <= this.map.Rank
                && coordinates.Y > 0 && coordinates.Y <= this.map.GetLength(coordinates.X-1);
        }
    }
}
