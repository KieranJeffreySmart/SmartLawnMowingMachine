namespace Slmm.Mower.Behaviour.Tests
{
    using Xbehave;
    using FluentAssertions;
    using slmm.LawnMowing.Model;

    public class StartupMowerFeature
    {
        public class StartupTestContext
        {
            public Garden Garden { get; internal set; }
            public Position StartingPosition { get; internal set; }
            public Mower Mower { get; internal set; }
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
                .x(() => context.Mower.Start(context.StartingPosition));
            "Then the Mower starts sucesfully"
                .x(() => context.Mower.HasStarted.Should().BeTrue());
        }

        [Scenario]
        [Example(2, 2, 0, 0)]
        [Example(2, 2, 0, 1)]
        [Example(2, 2, 1, 0)]
        [Example(2, 2, 3, 1)]
        [Example(2, 2, 1, 3)]
        [Example(2, 2, 3, 3)]
        [Example(5, 5, 6, 6)]
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
                .x(() => context.Mower.Start(context.StartingPosition));
            "Then the Mower should not start"
                .x(() => context.Mower.HasStarted.Should().BeFalse());
        }

        [Scenario]
        public void StartAMowerThatHasAlreadyBeenStarted()
        {
            var context = new StartupTestContext();

            "Given I have a garden"
                .x(() => context.Garden = new Garden(1, 1));
            "And I have a Mower"
                .x(() => context.Mower = new Mower(context.Garden));
            "And I have a position I want the mower to start in"
                .x(() => context.StartingPosition = new Position(new Coordinates(1, 1), Orientation.South));
            "And I have started the Mower"
                .x(() => context.Mower.Start(context.StartingPosition));
            "When I start the Mower again"
                .x(() => context.Mower.Start(context.StartingPosition));
            "Then the Mower should not start"
                .x(() => context.Mower.HasStarted.Should().BeTrue());
        }
    }
}
