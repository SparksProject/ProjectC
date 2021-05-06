using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chep.Service.Interface;
using System;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Chep.WebApi.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class DefinitionController : ControllerBase
    {
        private readonly IDefinitionService _service;
        //private IWebHostEnvironment _environment;

        public DefinitionController(IDefinitionService service, IWebHostEnvironment environment)
        {
            _service = service;
            //_environment = environment;
        }

        [HttpGet("GetPeriodTypes")]
        public IActionResult GetPeriodTypes()
        {
            try
            {
                var result = _service.GetPeriodTypes().Result;

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                //var basePath = Path.Combine(_environment.WebRootPath, "files");
                var filename = "logs.txt";

                var message = string.Empty;

                if (ex.Message != null)
                {
                    message += ex.Message;
                    message += Environment.NewLine;
                }

                if (ex.InnerException != null)
                {
                    message += ex.InnerException.ToString();
                    message += Environment.NewLine;
                }

                if (ex.StackTrace != null)
                {
                    message += ex.StackTrace;
                    message += Environment.NewLine;
                }

                //System.IO.File.AppendAllText(Path.Combine(basePath, filename), message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
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

        [HttpGet("GetCustoms")]
        public IActionResult GetCustoms()
        {
            var result = _service.GetCustoms().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }
        [HttpGet("GetUnits")]
        public IActionResult GetUnits()
        {
            var result = _service.GetUnits().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetNextReferenceNumber/{id}")]
        public IActionResult GetNextReferenceNumber(string id)
        {
            var result = _service.GetNextReferenceNumber(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetCountries")]
        public IActionResult GetCountries()
        {
            var result = _service.GetCountries().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetCurrencyTypes")]
        public IActionResult GetCurrencyTypes()
        {
            var result = _service.GetCurrencyTypes().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts()
        {
            var result = _service.GetProducts().Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}