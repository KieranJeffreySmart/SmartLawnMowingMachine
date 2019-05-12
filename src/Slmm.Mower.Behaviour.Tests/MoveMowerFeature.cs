
namespace Slmm.Mower.Behaviour.Tests
{
    using FluentAssertions;
    using Slmm.Api;
    using Slmm.Domain;
    using System;
    using System.Threading.Tasks;
    using Xbehave;

    public class MoveMowerFeature
    {
        public class MoveMowerTestContext
        {
            public Garden Garden { get; internal set; }
            public Mower Mower { get; internal set; }
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
            "When I move the Mower forward"
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
            "When I move the Mower forward"
                .x(() => context.Mower.Move());
            "Then its position should not change"
                .x(() => this.AssertPosition(context.Mower.GetPosition(), currentPosition.Coordinates.X, currentPosition.Coordinates.Y, currentPosition.Orientation));
        }

        [Scenario]
        [Example(2, 2, 1, 1, Orientation.North, TurnDirection.Clockwise, Orientation.East)]
        [Example(2, 2, 1, 1, Orientation.West, TurnDirection.Clockwise, Orientation.North)]
        [Example(2, 2, 2, 2, Orientation.South, TurnDirection.Clockwise, Orientation.West)]
        [Example(2, 2, 2, 2, Orientation.East, TurnDirection.Clockwise, Orientation.South)]
        [Example(2, 2, 1, 1, Orientation.North, TurnDirection.AntiClockwise, Orientation.West)]
        [Example(2, 2, 1, 1, Orientation.West, TurnDirection.AntiClockwise, Orientation.South)]
        [Example(2, 2, 2, 2, Orientation.South, TurnDirection.AntiClockwise, Orientation.East)]
        [Example(2, 2, 2, 2, Orientation.East, TurnDirection.AntiClockwise, Orientation.North)]
        public void TurnMower(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation, TurnDirection turnDirection, Orientation expectedOrientation)
        {
            var context = new MoveMowerTestContext();
            Position currentPosition = new Position(new Coordinates(startX, startY), orientation);
            "Given I have a garden"
                .x(() => context.Garden = new Garden(gardenLength, gardenWidth));
            "And I have started a new Mower inside the garden"
                .x(() => context.Mower = this.CreateNewMower(context.Garden, startX, startY, orientation));
            "When I turn the Mower"
                .x(() => context.Mower.Turn(turnDirection));
            "Then its orientation should be as expected"
                .x(() => context.Mower.GetPosition().Orientation.Should().Be(expectedOrientation));
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

    public class AsynchronousMowerTests
    {
        public class AsynchronousMowerTestContext
        {
            public SmartLawnMowingMachineService Service { get; internal set; }
            public Position StartPosition { get; internal set; }
        }

        AsynchronousMowerTestContext context = new AsynchronousMowerTestContext();

        [Background]
        public void SetupMowerService()
        {
            int gardenLength = 2;
            int gardenWidth = 2;
            int startX = 1;
            int startY = 1;
            Orientation orientation = Orientation.South;

            "Given I have a service"
                .x(() => context.Service = new SmartLawnMowingMachineService());
            "And a Mower was started"
                .x(() => SmartLawnMowingMachineService.Mower = this.StartMower(gardenLength, gardenWidth, startX, startY, orientation));
            "Given I have got the Mower's position"
                .x(async () => context.StartPosition = await context.Service.GetPosition());
        }

        [Scenario]
        public void GetPositionWhileTurningMower()
        {
            TurnDirection turnDirection = TurnDirection.Clockwise;
            Orientation expectedOrientation = Orientation.West;
            Task turnMotorTask = null;
            Position currentPosition = null;

            "And I have requested the Mower turn"
                .x(async () => turnMotorTask = context.Service.Turn(turnDirection));
            "When I get the Mowers position before the work is complete"
                .x(async () => currentPosition = await context.Service.GetPosition());
            "Then its orientation should not change"
                .x(() => this.AssertPosition(currentPosition, context.StartPosition.Coordinates.X, context.StartPosition.Coordinates.Y, context.StartPosition.Orientation));
            "When I get the Mowers position after the work is complete"
                .x(async () => currentPosition = await this.GetPositionAfterTask(context.Service, turnMotorTask));
            "Then its orientation should be as expected"
                .x(() => this.AssertPosition(currentPosition, context.StartPosition.Coordinates.X, context.StartPosition.Coordinates.Y, expectedOrientation));
        }

        [Scenario]
        public void GetPositionWhileMovingMower()
        {
            Coordinates expectedCoordinates = new Coordinates(1, 2);
            Task turnMotorTask = null;
            Position currentPosition = null;

            "And I have requested the Mower move"
                .x(async () => turnMotorTask = context.Service.Move());
            "When I get the Mowers position before the work is complete"
                .x(async () => currentPosition = await context.Service.GetPosition());
            "Then its position should not change"
                .x(() => this.AssertPosition(currentPosition, context.StartPosition.Coordinates.X, context.StartPosition.Coordinates.Y, context.StartPosition.Orientation));
            "When I get the Mowers position after the work is complete"
                .x(async () => currentPosition = await this.GetPositionAfterTask(context.Service, turnMotorTask));
            "Then its position should be as expected"
                .x(() => this.AssertPosition(currentPosition, expectedCoordinates.X, expectedCoordinates.Y, context.StartPosition.Orientation));
        }

        private async Task<Position> GetPositionAfterTask(SmartLawnMowingMachineService service, Task task)
        {
            task.Wait();
            return await service.GetPosition();
        }

        private Mower StartMower(int gardenLength, int gardenWidth, int startX, int startY, Orientation orientation)
        {
            var mower = new Mower(new Garden(gardenLength, gardenWidth));
            mower.Start(new Position(new Coordinates(startX, startY), orientation));
            return mower;
        }

        private Mower CreateNewMower(Garden garden, int startX, int startY, Orientation orientation)
        {
            var newMower = new Mower(garden);
            newMower.Start(new Position(new Coordinates(startX, startY), orientation));
            return newMower;
        }

        private void AssertPosition(Position currentPosition, int posX, int posY, Orientation orientation)
        {
            currentPosition.Should().NotBeNull();
            currentPosition.Coordinates.X.Should().Be(posX);
            currentPosition.Coordinates.Y.Should().Be(posY);
            currentPosition.Orientation.Should().Be(orientation);
        }
    }
}
