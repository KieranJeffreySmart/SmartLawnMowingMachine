namespace Slmm.WebApplication.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Slmm.Api;
    using Slmm.Api.Presentation.Dtos;

    [Route("api/[controller]")]
    [ApiController]
    public class MowerController : ControllerBase
    {
        AsyncSmartLawnMowingMachineService service;

        public MowerController(AsyncSmartLawnMowingMachineService service)
        {
            this.service = service;
        }

        // GET api/values
        [HttpGet]
        [Route("position")]
        public async Task<ActionResult<PositionDto>> Get()
        {
            return new ActionResult<PositionDto>(await this.service.GetPosition());
        }

        // POST api/values
        [HttpPost]
        [Route("move")]
        public async Task<ActionResult> MoveMower()
        {
            var result = await this.service.Move();

            if (result == MowerResponseResult.InvalidInput)
            {
                return BadRequest();
            }

            if (result == MowerResponseResult.IsBusy)
            {
                return Conflict("The Mower Is Busy");
            }

            return Ok();
        }

        // POST api/values
        [HttpPost]
        [Route("turn")]
        public async Task<ActionResult> TurnMower([FromBody] string orientation)
        {
            var result = await this.service.Turn(orientation);

            if (result == MowerResponseResult.InvalidInput)
            {
                return BadRequest();
            }

            if (result == MowerResponseResult.IsBusy)
            {
                return Conflict("The Mower Is Busy");
            }

            return Ok();
        }
    }
}
