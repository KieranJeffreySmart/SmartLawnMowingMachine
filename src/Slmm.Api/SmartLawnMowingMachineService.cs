namespace Slmm.Api
{
    using Slmm.Domain;
    using System.Threading.Tasks;

    public class SmartLawnMowingMachineService
    {
        public static Mower Mower;

        public async Task Turn(TurnDirection direction)
        {
            await Task.Run(() => Mower.Turn(direction));
        }

        public async Task Move()
        {
            await Task.Run(() => Mower.Move());
        }

        public async Task<Position> GetPosition()
        {
            return await Task.Run(() => Mower.GetPosition());
        }
    }
}
