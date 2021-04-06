using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProjectC.DTO;
using ProjectC.Service.Interface;

namespace ProjectC.Api.Controllers
{
    [Route("api/[controller]")]
    public class StokCikisController : Controller
    {
        private readonly IStokCikisService _service;
        private readonly IStokGirisService _serviceGiris;

        public StokCikisController(IStokCikisService service, IStokGirisService serviceGiris)
        {
            _service = service;
            _serviceGiris = serviceGiris;
        }


        // Crud
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
        public IActionResult Post([FromBody] ChepStokCikisDTO obj)
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
        public IActionResult Edit([FromBody] ChepStokCikisDTO obj)
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

        [HttpGet("GetGirisDetayList/{id}")]
        public IActionResult GetGirisDetayList(int id)
        {
            var result = _serviceGiris.Get(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}