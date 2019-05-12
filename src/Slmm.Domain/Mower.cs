using System;
using System.Collections.Generic;

namespace Slmm.Domain
{
    public class Mower
    {
        private Garden garden;
        private Position position;
        private IDictionary<Orientation, Func<Position, Position>> getPositionCommands = new Dictionary<Orientation, Func<Position, Position>>
        {
            {
                Orientation.North,
                (currentPosition) => new Position(new Coordinates(currentPosition.Coordinates.X, currentPosition.Coordinates.Y-1), currentPosition.Orientation)
            },
            {
                Orientation.South,
                (currentPosition) => new Position(new Coordinates(currentPosition.Coordinates.X, currentPosition.Coordinates.Y+1), currentPosition.Orientation)
            },
            {
                Orientation.East,
                (currentPosition) => new Position(new Coordinates(currentPosition.Coordinates.X+1, currentPosition.Coordinates.Y), currentPosition.Orientation)
            },
            {
                Orientation.West,
                (currentPosition) => new Position(new Coordinates(currentPosition.Coordinates.X-1, currentPosition.Coordinates.Y), currentPosition.Orientation)
            }
        };

        private IDictionary<Orientation, Func<TurnDirection, Orientation>> getNewOrientationCommands = new Dictionary<Orientation, Func<TurnDirection, Orientation>>
        {
            {
                Orientation.North,
                (direction) => direction == TurnDirection.Clockwise ?  Orientation.East : Orientation.West
            },
            {
                Orientation.South,
                (direction) => direction == TurnDirection.Clockwise ?  Orientation.West : Orientation.East
            },
            {
                Orientation.East,
                (direction) => direction == TurnDirection.Clockwise ?  Orientation.South : Orientation.North
            },
            {
                Orientation.West,
                (direction) => direction == TurnDirection.Clockwise ?  Orientation.North : Orientation.South
            }
        };

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

        public void Move()
        {
            var nextPosition = this.GetNextPosition();
            if (this.garden.CellIsInsideGarden(nextPosition.Coordinates))
            {
                this.position = nextPosition;
            }
        }

        private Position GetNextPosition()
        {
            var command = getPositionCommands[this.position.Orientation];

            if (command == null)
            {
                throw new ArgumentOutOfRangeException($"No command exists for the orientation {this.position.Orientation} to get the next position");
            }

            return command(this.position);
        }

        public void Turn(TurnDirection turnDirection)
        {
            var command = getNewOrientationCommands[this.position.Orientation];

            if (command == null)
            {
                throw new ArgumentOutOfRangeException($"No command exists for the orientation {this.position.Orientation} to get a new orientation");
            }

            this.position = new Position(this.position.Coordinates.Clone() as Coordinates, command(turnDirection));
        }
    }
}
