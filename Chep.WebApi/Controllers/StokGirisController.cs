using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Chep.DTO;
using Chep.Service.Interface;

namespace Chep.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class StokGirisController : Controller
    {
        private readonly IStokGirisService _service;

        public StokGirisController(IStokGirisService service)
        {
            _service = service;
        }


        // Crud
        [HttpGet("List")]
        public IActionResult List([FromQuery] int? referansNo, [FromQuery] string beyannameNo, [FromQuery] string tpsNo)
        {
            var result = _service.List(referansNo, beyannameNo, tpsNo);

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
        public IActionResult Post([FromBody] ChepStokGirisDTO obj)
        {
            var result = _service.Add(obj);

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
                case Enums.ResponseMessage.BADREQUEST:
                    return StatusCode(StatusCodes.Status400BadRequest);
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
        public IActionResult Edit([FromBody] ChepStokGirisDTO obj)
        {

            var result = _service.Edit(obj);

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

        // Detail
        [HttpGet("ListDetails")]
        public IActionResult ListDetails()
        {
            var result = _service.ListDetails();

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


        [HttpGet("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _service.Delete(id);

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
                case Enums.ResponseMessage.WARNING:
                    return StatusCode(StatusCodes.Status500InternalServerError,result.Message);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }


        [HttpPost("Import")]
        public IActionResult Import([FromQuery] int userId)
        {
            var result = new ResponseDTO();

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    result = _service.Import(file, userId);
                }
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }

            return result.ResultMessage switch
            {
                Enums.ResponseMessage.OK => StatusCode(StatusCodes.Status200OK, result.Result),
                Enums.ResponseMessage.ERROR => StatusCode(StatusCodes.Status500InternalServerError, result.Exception.Message),
                Enums.ResponseMessage.NOTFOUND => StatusCode(StatusCodes.Status404NotFound),
                Enums.ResponseMessage.UNAUTHORIZED => StatusCode(StatusCodes.Status401Unauthorized),
                Enums.ResponseMessage.BADREQUEST => StatusCode(StatusCodes.Status400BadRequest),
                Enums.ResponseMessage.WARNING => StatusCode(StatusCodes.Status200OK, result),
                _ => StatusCode(StatusCodes.Status500InternalServerError, result),
            };
        }

    }
}