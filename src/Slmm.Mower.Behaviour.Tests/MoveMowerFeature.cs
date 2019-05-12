
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
            public Position CurrentPosition { get; internal set; }
            public Mower Mower { get; internal set; }
            public bool StartMowerResult { get; internal set; }
        }

        [Scenario]
        [Example(2, 2, 1, 1, Orientation.South)]
        [Example(2, 2, 2, 2, Orientation.South)]
        public void GetPositionFromStartup(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation)
        {
            var context = new MoveMowerTestContext();
            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have started a new Mower inside the garden and facing south"
                .x(() => context.Mower = this.CreateNewMower(context.Garden, startX, startY, orientation));
            "When I get the Mower's position"
                .x(() => context.CurrentPosition = context.Mower.GetPosition());
            "Then it should be as I set it"
                .x(() => this.AssertPosition(context.CurrentPosition, startX, startY, orientation));
        }

        private void AssertPosition(Position currentPosition, int posX, int posY, Orientation orientation)
        {
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
