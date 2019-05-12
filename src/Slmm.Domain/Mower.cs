using System;

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

        public bool HasStarted { get; private set; }

        public void Start(Position startingPosition)
        {
            this.position = startingPosition;
            this.HasStarted = this.garden.CellIsInsideGarden(startingPosition.Coordinates);
        }

        public Position GetPosition()
        {
            return this.position.Clone() as Position;
        }
    }
}
