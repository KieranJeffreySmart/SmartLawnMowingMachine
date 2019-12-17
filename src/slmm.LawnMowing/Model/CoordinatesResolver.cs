namespace slmm.LawnMowing.Model
{
    using System;
    using System.Collections.Generic;

    public class CoordinatesResolver
    {
        private IDictionary<Orientation, Func<Coordinates, Coordinates>> getCoordinatesCommands = new Dictionary<Orientation, Func<Coordinates, Coordinates>>
        {
            {
                Orientation.North,
                (currentCoordinates) => new Coordinates(currentCoordinates.X, currentCoordinates.Y-1)
            },
            {
                Orientation.South,
                (currentCoordinates) => new Coordinates(currentCoordinates.X, currentCoordinates.Y+1)
            },
            {
                Orientation.East,
                (currentCoordinates) => new Coordinates(currentCoordinates.X+1, currentCoordinates.Y)
            },
            {
                Orientation.West,
                (currentCoordinates) => new Coordinates(currentCoordinates.X-1, currentCoordinates.Y)
            }
        };

        public Coordinates GetNextCoordinates(Position currentPosition)
        {
            var command = getCoordinatesCommands[currentPosition.Orientation];

            if (command == null)
            {
                throw new ArgumentOutOfRangeException($"No command exists for the orientation {currentPosition.Orientation} to get the next position");
            }

            return command(currentPosition.Coordinates);
        }
    }
}
