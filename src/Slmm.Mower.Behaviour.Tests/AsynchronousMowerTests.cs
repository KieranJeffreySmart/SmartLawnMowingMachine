namespace Slmm.Mower.Behaviour.Tests
{
    using FluentAssertions;
    using Moq;
    using Slmm.Api;
    using Slmm.Api.Infrastructure;
    using Slmm.Api.Presentation.Dtos;
    using Slmm.Domain;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Xbehave;

    public class AsynchronousMowerTests
    {
        public class AsynchronousMowerTestContext
        {
            public AsyncSmartLawnMowingMachineService Service { get; internal set; }
            public PositionDto StartPosition { get; internal set; }
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

            var settingsProvider = new Mock<ISettingsResolver>();
            settingsProvider.Setup(sr =>
                sr.ReadValue<int>(
                    MowerFactory.GardenDimentionsSettingsSectionName,
                    MowerFactory.GardenLengthSettingsKeyName,
                    MowerFactory.GardenLengthSettingsDefaultValue)).Returns(gardenLength);

            settingsProvider.Setup(sr =>
                sr.ReadValue<int>(
                    MowerFactory.GardenDimentionsSettingsSectionName,
                    MowerFactory.GardenWidthSettingsKeyName,
                    MowerFactory.GardenWidthSettingsDefaultValue)).Returns(gardenWidth);

            settingsProvider.Setup(sr =>
                sr.ReadValue<int>(
                    MowerFactory.MowerStartupSettingsSectionName,
                    MowerFactory.MowerXPosSettingsKeyName,
                    MowerFactory.MowerXPosSettingsDefaultValue)).Returns(startX);

            settingsProvider.Setup(sr =>
                sr.ReadValue<int>(
                    MowerFactory.MowerStartupSettingsSectionName,
                    MowerFactory.MowerYPosSettingsKeyName,
                    MowerFactory.MowerYPosSettingsDefaultValue)).Returns(startY);

            settingsProvider.Setup(sr =>
                sr.ReadValue(
                    MowerFactory.MowerStartupSettingsSectionName,
                    MowerFactory.MowerOrientationSettingsKeyName,
                    MowerFactory.MowerOrientationSettingsDefaultValue)).Returns(orientation.ToString());

            "Given I have a service"
                .x(() => this.context.Service = new AsyncSmartLawnMowingMachineService(new MowerFactory(settingsProvider.Object)));
            "And a Mower was started"
                .x(() => this.StartMower(gardenLength, gardenWidth, startX, startY, orientation));
            "And I have got the Mower's position"
                .x(async () => this.context.StartPosition = await this.context.Service.GetPosition());
        }

        [Scenario]
        public void GetPositionWhileTurningMower()
        {
            string turnDirection = TurnDirection.Clockwise.ToString();
            string expectedOrientation = Orientation.West.ToString();
            Task turnMotorTask = null;
            PositionDto currentPosition = null;

            "Given I have requested the Mower turn"
                .x(async () => turnMotorTask = this.context.Service.Turn(turnDirection));
            "When I get the Mowers position before the work is complete"
                .x(async () => currentPosition = await this.GetPositionBeforeWorkIsComplete());
            "Then its orientation should not change"
                .x(() => this.AssertPosition(currentPosition, this.context.StartPosition.X, this.context.StartPosition.Y, this.context.StartPosition.Orientation));
            "When I get the Mowers position after the work is complete"
                .x(async () => currentPosition = await this.GetPositionAfterTask(this.context.Service, turnMotorTask));
            "Then its orientation should be as expected"
                .x(() => 
                this.AssertPosition(currentPosition, this.context.StartPosition.X, this.context.StartPosition.Y, expectedOrientation));
        }

        [Scenario]
        public void GetPositionWhileMovingMower()
        {
            Coordinates expectedCoordinates = new Coordinates(1, 2);
            Task turnMotorTask = null;
            PositionDto currentPosition = null;

            "Given I have requested the Mower move"
                .x(async () => turnMotorTask = this.context.Service.Move());
            "When I get the Mowers position before the work is complete"
                .x(async () => currentPosition = await this.context.Service.GetPosition());
            "Then its position should not change"
                .x(() => this.AssertPosition(currentPosition, this.context.StartPosition.X, this.context.StartPosition.Y, this.context.StartPosition.Orientation));
            "When I get the Mowers position after the work is complete"
                .x(async () => currentPosition = await this.GetPositionAfterTask(this.context.Service, turnMotorTask));
            "Then its position should be as expected"
                .x(() => this.AssertPosition(currentPosition, expectedCoordinates.X, expectedCoordinates.Y, this.context.StartPosition.Orientation));
        }

        [Scenario]
        public void MoveWhileMovingMower()
        {
            Coordinates expectedCoordinates = new Coordinates(1, 2);
            Task moveMotorTask = null;
            PositionDto currentPosition = null;
            var moveResult = MowerResponseResult.Success;

            "Given I have requested the Mower move"
                .x(async () => moveMotorTask = this.context.Service.Move());
            "When I move the Mower before the work is complete"
                .x(async () => moveResult = await this.MoveBeforeWorkIsComplete(this.context.Service));
            "Then it should not be sucessfull"
                .x(() => moveResult.Should().Be(MowerResponseResult.IsBusy));
            "When I move the Mower before the work is complete"
                .x(async () => moveResult = await this.MoveAfterTask(this.context.Service, moveMotorTask));
            "Then it should be sucessfull"
                .x(() => moveResult.Should().Be(MowerResponseResult.Success));
        }

        [Scenario]
        public void TurnWhileTurningMower()
        {
            string turnDirection = TurnDirection.Clockwise.ToString();
            Orientation expectedOrientation = Orientation.West;
            Task turnMotorTask = null;
            PositionDto currentPosition = null;
            var turnResult = MowerResponseResult.Success;

            "Given I have requested the Mower turn"
                .x(async () => turnMotorTask = this.context.Service.Turn(turnDirection));
            "When I turn the Mower before the work is complete"
                .x(async () => turnResult = await this.TurnBeforeWorkIsComplete(this.context.Service, turnDirection));
            "Then it should not be sucessfull"
                .x(() => turnResult.Should().Be(MowerResponseResult.IsBusy));
            "When I turn the Mower after the work is complete"
                .x(async () => turnResult = await this.TurnAfterTask(this.context.Service, turnMotorTask, turnDirection));
            "Then it should be sucessfull"
                .x(() => turnResult.Should().Be(MowerResponseResult.Success));
        }

        private async Task<PositionDto> GetPositionBeforeWorkIsComplete()
        {
            Thread.Sleep(200);
            return await this.context.Service.GetPosition();
        }

        private async Task<MowerResponseResult> MoveBeforeWorkIsComplete(AsyncSmartLawnMowingMachineService service)
        {
            Thread.Sleep(200);
            return await service.Move();
        }

        private async Task<MowerResponseResult> TurnBeforeWorkIsComplete(AsyncSmartLawnMowingMachineService service, string turnDirection)
        {
            Thread.Sleep(200);
            return await service.Turn(turnDirection);
        }

        private async Task<MowerResponseResult> MoveAfterTask(AsyncSmartLawnMowingMachineService service, Task task)
        {
            task.Wait();
            return await service.Move();
        }

        private async Task<MowerResponseResult> TurnAfterTask(AsyncSmartLawnMowingMachineService service, Task task, string turnDirection)
        {
            task.Wait();
            return await service.Turn(turnDirection);
        }

        private async Task<PositionDto> GetPositionAfterTask(AsyncSmartLawnMowingMachineService service, Task task)
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

        private void AssertPosition(PositionDto currentPosition, int posX, int posY, string orientation)
        {
            currentPosition.Should().NotBeNull();
            currentPosition.X.Should().Be(posX);
            currentPosition.Y.Should().Be(posY);
            currentPosition.Orientation.Should().Be(orientation);
        }
    }
}
