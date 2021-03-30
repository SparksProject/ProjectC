using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SparksX.DTO;
using SparksX.Service.Interface;

namespace SparksX.Api.Controllers
{
    [Route("api/[controller]")]
    public class MailReportController : Controller
    {
        private readonly IMailReportService _service;

        public MailReportController(IMailReportService service)
        {
            _service = service;
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _service.List();

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost("Add")]
        public IActionResult Post(MailReportDTO request)
        {
            var result = _service.Add(request);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get(int id)
        {
            var result = _service.Get(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("Edit")]
        public IActionResult Edit(MailReportDTO request)
        {
            var result = _service.Edit(request);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result.Result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("GetMailDefinitions")]
        public IActionResult GetMailDefinitions()
        {
            var result = _service.GetMailDefinitions();

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("AddMailDefinition")]
        public IActionResult AddMailDefinition(MailDefinitionDto obj)
        {
            var result = _service.AddMailDefinition(obj);

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet("GetMailResultSet")]
        public IActionResult GetMailResultSet(int id, int userId)
        {
            var result = _service.GetMailResultSet(id, userId);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK: return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.WARNING: return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.ERROR:
                default: return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }
    }
}