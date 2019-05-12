namespace Slmm.Domain
{
    public class Garden
    {
        private readonly int[,] map;

        public Garden(int length, int width)
        {
            this.map = new int[length,width];
        }

        internal bool CellIsInsideGarden(Coordinates coordinates)
        {
            return coordinates.X > 0 && coordinates.X <= this.map.GetLength(1)
                && coordinates.Y > 0 && coordinates.Y <= this.map.GetLength(0);
        }
    }
}
