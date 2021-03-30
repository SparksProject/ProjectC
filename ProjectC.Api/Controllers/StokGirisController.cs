using System;
using System.Data;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProjectC.Api.Models;
using ProjectC.DTO;
using ProjectC.Service.Interface;

using Syncfusion.XlsIO;


namespace ProjectC.Api.Controllers
{
    [Route("api/[controller]")]
    public class StokGirisController : Controller
    {
        private readonly IStokGirisService _service;

        public StokGirisController(IStokGirisService service)
        {
            _service = service;
        }


        // CRUD
        [HttpGet("List/{id}")]
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
        public IActionResult Post(ChepStokGirisDTO obj)
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
        public IActionResult Edit(ChepStokGirisDTO obj)
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
    }
}