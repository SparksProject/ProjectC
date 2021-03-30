using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SparksX.Service.Interface;

namespace SparksX.Api.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DefinitionController : ControllerBase
    {
        private readonly IDefinitionService _service;

        public DefinitionController(IDefinitionService service)
        {
            _service = service;
        }

        [HttpGet("GetPeriodTypes")]
        public IActionResult GetPeriodTypes()
        {
            var result = _service.GetPeriodTypes().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetRecordStatuses")]
        public IActionResult GetRecordStatuses()
        {
            var result = _service.GetRecordStatuses().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            var result = _service.GetUsers().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetCustomers")]
        public IActionResult GetCustomers()
        {
            var result = _service.GetCustomers().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetMailDefinitions")]
        public IActionResult GetMailDefinitions()
        {
            var result = _service.GetMailDefinitions().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetUserTypes")]
        public IActionResult GetUserTypes()
        {
            var result = _service.GetUserTypes().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetParameterTypes")]
        public IActionResult GetParameterTypes()
        {
            var result = _service.GetParameterTypes().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}