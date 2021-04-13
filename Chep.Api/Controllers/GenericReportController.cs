using System.Collections.Generic;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Chep.Api.Models;
using Chep.DTO;
using Chep.Service.Interface;

namespace Chep.Api.Controllers
{
    [Route("api/[controller]")]
    public class GenericReportController : ControllerBase
    {
        private readonly IGenericReportService _service;

        private readonly IWebHostEnvironment _hostingenviroment;

        public GenericReportController(IGenericReportService service, IWebHostEnvironment hostingenvironment)
        {
            _service = service;
            _hostingenviroment = hostingenvironment;
        }

        [HttpGet("List/{id}")]
        public IActionResult List(int id)
        {
            var result = _service.List(id);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] GenericReportDTO request)
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
        public IActionResult Edit([FromBody] GenericReportDTO request)
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

        [HttpPost("GetResultSet")]
        public IActionResult GetResultSet([FromBody] GenericReportDTO obj)
        {
            var result = _service.GetResultSet(obj.GenericReportId, obj.UserId, obj.GenericReportParameterList);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK: return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.WARNING: return StatusCode(StatusCodes.Status200OK, result);

                default: return StatusCode(StatusCodes.Status500InternalServerError, result);
            }
        }

        //[HttpPost("CreateExcel")]
        //public IActionResult CreateExcel([FromBody] GenericReportDTO obj)
        //{
        //    var result = _service.CreateExcel(obj.GenericReportId, obj.GenericReportParameterList);

        //    switch (result.ResultMessage)
        //    {
        //        case Enums.ResponseMessage.OK:
        //            {
        //                var excelData = result.Result as byte[];

        //                if (string.IsNullOrEmpty(obj.GenericReportName))
        //                {
        //                    obj.GenericReportName = Guid.NewGuid().ToString();
        //                }

        //                var fileName = $"{obj.GenericReportName}.xlsx";
        //                var fullPath = Path.Combine(_hostingenviroment.ContentRootPath, "files", fileName);
        //                System.IO.File.WriteAllBytes(fullPath, excelData);

        //                obj.GenericReportName = fileName;

        //                return StatusCode(StatusCodes.Status200OK, obj);
        //            }
        //        case Enums.ResponseMessage.ERROR: return StatusCode(StatusCodes.Status500InternalServerError, result.Result);
        //        default: return StatusCode(StatusCodes.Status500InternalServerError, result.Result);
        //    }
        //}
    }
}