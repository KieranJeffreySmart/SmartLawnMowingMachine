
namespace Slmm.Mower.Behaviour.Tests
{
    using System;
    using FluentAssertions;
    using Slmm.Domain;
    using Xbehave;

    public class MoveMowerFeature
    {
        public class MoveMowerTestContext
        {
            public Garden Garden { get; internal set; }
            public Mower Mower { get; internal set; }
            public bool StartMowerResult { get; internal set; }
        }

        [Scenario]
        [Example(2, 2, 1, 1, Orientation.South)]
        [Example(2, 2, 2, 2, Orientation.South)]
        public void GetPositionFromStartup(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation)
        {
            var context = new MoveMowerTestContext();
            Position currentPosition = null;
            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have started a new Mower inside the garden"
                .x(() => context.Mower = this.CreateNewMower(context.Garden, startX, startY, orientation));
            "When I get the Mower's position"
                .x(() => currentPosition = context.Mower.GetPosition());
            "Then it should be as I set it"
                .x(() => this.AssertPosition(currentPosition, startX, startY, orientation));
        }

        [Scenario]
        [Example(2, 2, 1, 1, Orientation.South, 1, 2)]
        [Example(2, 2, 1, 1, Orientation.East, 2, 1)]
        [Example(2, 2, 2, 2, Orientation.North, 2, 1)]
        [Example(2, 2, 2, 2, Orientation.West, 1, 2)]
        public void MoveMowerToAValidCell(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation, int expectedX, int expectedY)
        {
            var context = new MoveMowerTestContext();
            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have started a new Mower inside the garden"
                .x(() => context.Mower = this.CreateNewMower(context.Garden, startX, startY, orientation));
            "When I move Mower forward"
                .x(() => context.Mower.Move());
            "Then its position should change as expected"
                .x(() => this.AssertPosition(context.Mower.GetPosition(), expectedX, expectedY, orientation));
        }

        [Scenario]
        [Example(2, 2, 1, 1, Orientation.North)]
        [Example(2, 2, 1, 1, Orientation.West)]
        [Example(2, 2, 2, 2, Orientation.South)]
        [Example(2, 2, 2, 2, Orientation.East)]
        public void MoveMowerToAnInvalidCell(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation)
        {
            var context = new MoveMowerTestContext();
            Position currentPosition = new Position(new Coordinates(startX, startY), orientation);
            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have started a new Mower inside the garden"
                .x(() => context.Mower = this.CreateNewMower(context.Garden, startX, startY, orientation));
            "When I move Mower forward"
                .x(() => context.Mower.Move());
            "Then its position should not change"
                .x(() => this.AssertPosition(context.Mower.GetPosition(), currentPosition.Coordinates.X, currentPosition.Coordinates.Y, currentPosition.Orientation));
        }

        private void AssertPosition(Position currentPosition, int posX, int posY, Orientation orientation)
        {
            currentPosition.Should().NotBeNull();
            currentPosition.Coordinates.X.Should().Be(posX);
            currentPosition.Coordinates.Y.Should().Be(posY);
            currentPosition.Orientation.Should().Be(orientation);
        }

        private Mower CreateNewMower(Garden garden, int startX, int startY, Orientation orientation)
        {
            var newMower = new Mower(garden);
            newMower.Start(new Position(new Coordinates(startX, startY), orientation));
            return newMower;
        }
    }
}
