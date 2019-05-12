namespace Slmm.Mower.Behaviour.Tests
{
    using Xbehave;
    using FluentAssertions;
    using Slmm.Domain;

    public class StartupMowerFeature
    {
        public class StartupTestContext
        {
            public Garden Garden { get; internal set; }
            public Position StartingPosition { get; internal set; }
            public Mower Mower { get; internal set; }
            public bool StartMowerResult { get; internal set; }
        }
                
        [Scenario]
        [Example(2, 2, 1, 1)]
        [Example(2, 2, 2, 2)]
        public void CreateAMower(int gardenLength, int gardenWidth, int startX, int startY)
        {
            var context = new StartupTestContext();

            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have a Mower"
                .x(() => context.Mower = new Mower(context.Garden));
            "And I have a position I want the mower to start in"
                .x(() => context.StartingPosition = new Position(new Coordinates(startX, startY), Orientation.South));
            "When I start the Mower"
                .x(() => context.StartMowerResult = context.Mower.Start(context.StartingPosition));
            "Then the Mower starts sucesfully"
                .x(() => context.StartMowerResult.Should().BeTrue());
        }

        [Scenario]
        [Example(2, 2, 0, 0)]
        [Example(2, 2, 0, 1)]
        [Example(2, 2, 1, 0)]
        [Example(2, 2, 3, 1)]
        [Example(2, 2, 1, 3)]
        [Example(2, 2, 3, 3)]
        public void CreateAMowerOutsideTheGarden(int gardenLength, int gardenWidth, int startX, int startY)
        {
            var context = new StartupTestContext();

            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have a Mower"
                .x(() => context.Mower = new Mower(context.Garden));
            "And I have a position I want the mower to start in"
                .x(() => context.StartingPosition = new Position(new Coordinates(startX, startY), Orientation.South));
            "When I start the Mower"
                .x(() => context.StartMowerResult = context.Mower.Start(context.StartingPosition));
            "Then the Mower starts sucesfully"
                .x(() => context.StartMowerResult.Should().BeFalse());
        }
    }
}
