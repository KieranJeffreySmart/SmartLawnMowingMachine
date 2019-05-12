namespace Slmm.Api
{
    using Slmm.Api.Presentation;
    using Slmm.Api.Presentation.Dtos;
    using Slmm.Domain.Model;
    using Slmm.Domain.Exceptions;
    using Slmm.Domain.Factories;
    using System;
    using System.Threading.Tasks;

    public class AsyncSmartLawnMowingMachineService
    {
        private Mower mower;

        private Mower Mower
        {
            get
            {
                if (mower == null)
                {
                    mower = this.mowerFactory.Create();
                }

                return mower;
            }
        }

        private IMowerFactory mowerFactory;

        public AsyncSmartLawnMowingMachineService(IMowerFactory mowerFactory)
        {
            this.mowerFactory = mowerFactory;
        }

        public async Task<MowerResponseResult> Turn(string direction)
        {
            return await Task.Run(() =>
            {
                try
                {
                    if (!Enum.TryParse<TurnDirection>(direction, out var domainDirection))
                    {
                        return MowerResponseResult.InvalidInput;
                    }

                    this.Mower.Turn(domainDirection);
                    return MowerResponseResult.Success;
                }
                catch (MowerIsBusyException)
                {
                    return MowerResponseResult.IsBusy;
                }
            });
        }

        public async Task<MowerResponseResult> Move()
        {
            return await Task.Run(() =>
            {
                try
                {
                    this.Mower.Move();
                    return MowerResponseResult.Success;
                }
                catch(MowerIsBusyException)
                {
                    return MowerResponseResult.IsBusy;
                }
                catch(OutOfGardenBoundaryException)
                {
                    return MowerResponseResult.OutOfBoundary;
                }
            });
        }

        public async Task<PositionDto> GetPosition()
        {
            return await Task.Run(() => this.Mower.GetPosition().ToDto());
        }
    }
}
