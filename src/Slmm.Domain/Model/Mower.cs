namespace Slmm.Domain.Model
{
    using Slmm.Domain.Exceptions;
    using System.Threading;

    public class Mower
    {
        private Garden garden;
        private Position position;
        private CoordinatesResolver coordinatesResolver = new CoordinatesResolver();
        private OrientationResolver orientationResolver = new OrientationResolver();

        private const int MillisecondsToTurn = 2000;
        private const int MillisecondsToMove = 5000;

        public Mower(Garden garden)
        {
            this.garden = garden;
        }

        public bool HasStarted { get; private set; }
        public bool IsBusy { get; private set; }

        public void Start(Position startingPosition)
        {
            this.position = startingPosition;
            this.HasStarted = this.garden.CellIsInsideGarden(startingPosition.Coordinates);
        }

        public Position GetPosition()
        {
            return this.position.Clone() as Position;
        }

        public void Move()
        {
            if (this.IsBusy)
            {
                throw new MowerIsBusyException();
            }

            var nextPosition = this.GetNextPosition();
            if (!this.garden.CellIsInsideGarden(nextPosition.Coordinates))
            {
                throw new OutOfGardenBoundaryException();
            }

            this.SimulateWork(MillisecondsToMove);
            this.position = nextPosition;
        }

        public void Turn(TurnDirection turnDirection)
        {
            if (this.IsBusy)
            {
                throw new MowerIsBusyException();
            }

            var newOrientation = orientationResolver.GetNextOrientation(this.position.Orientation, turnDirection);

            this.SimulateWork(MillisecondsToTurn);

            this.position = new Position(this.position.Coordinates.Clone() as Coordinates, newOrientation);
        }

        private Position GetNextPosition()
        {
            return new Position(coordinatesResolver.GetNextCoordinates(this.position), this.position.Orientation);
        }

        private void SimulateWork(int millisecondsTaken)
        {
            this.IsBusy = true;
            Thread.Sleep(millisecondsTaken);
            this.IsBusy = false;
        }
    }
}
