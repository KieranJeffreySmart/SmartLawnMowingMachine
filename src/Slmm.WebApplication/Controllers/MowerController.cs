﻿namespace Slmm.WebApplication.Controllers
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

            return ResolveHttpResponse(result);
        }

        // POST api/values
        [HttpPost]
        [Route("turn")]
        public async Task<ActionResult> TurnMower([FromBody] string orientation)
        {
            var result = await this.service.Turn(orientation);
            
            return ResolveHttpResponse(result);
        }

        private ActionResult ResolveHttpResponse(MowerResponseResult mowerResponseResult)
        {
            if (mowerResponseResult == MowerResponseResult.InvalidInput)
            {
                return BadRequest();
            }

            if (mowerResponseResult == MowerResponseResult.IsBusy)
            {
                return Accepted("", "The Mower Is Busy");
            }

            if (mowerResponseResult == MowerResponseResult.OutOfBoundary)
            {
                return Accepted("", "The Mower cannot move beyond the garden boundary");
            }

            return Ok();
        }
    }
}
