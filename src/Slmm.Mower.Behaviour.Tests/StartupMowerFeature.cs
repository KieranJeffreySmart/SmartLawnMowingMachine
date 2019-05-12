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
        public void CreateAMower()
        {
            var context = new StartupTestContext();

            "Given I have a garden"
                .x(() => context.Garden = new Garden(2, 2));
            "And I have a Mower"
                .x(() => context.Mower = new Mower(context.Garden));
            "And I have a position I want the mower to start in"
                .x(() => context.StartingPosition = new Position(new Coordinates(1, 1), Orientation.South));
            "When I start the Mower"
                .x(() => context.StartMowerResult = context.Mower.Start(context.StartingPosition));
            "Then the Mower starts sucesfully"
                .x(() => context.StartMowerResult.Should().BeTrue());
        }
    }
}
