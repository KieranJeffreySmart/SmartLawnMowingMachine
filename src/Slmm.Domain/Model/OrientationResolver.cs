namespace Slmm.Domain.Model
{
    using System;
    using System.Collections.Generic;

    public class OrientationResolver
    {
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

        public Orientation GetNextOrientation(Orientation currentOrientation, TurnDirection direction)
        {

            var command = getNewOrientationCommands[currentOrientation];

            if (command == null)
            {
                throw new ArgumentOutOfRangeException($"No command exists for the orientation {currentOrientation} to get a new orientation");
            }

            return command(direction);
        }
    }
}
