using System;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using SparksX.DTO;
using SparksX.Service.Interface;
namespace SparksX.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _service;

        private readonly IHostingEnvironment _hostingenvironment;

        public InvoiceDetailController(IInvoiceDetailService service, IHostingEnvironment hostingenvironment)
        {
            _service = service;
            _hostingenvironment = hostingenvironment;
        }

        [HttpGet("List/{id}")]
        public IActionResult List(Guid id)
        {
            var result = _service.List(id);

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