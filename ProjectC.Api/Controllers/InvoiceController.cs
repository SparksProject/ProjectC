using System;
using System.Linq;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using ProjectC.Data.Models;
using ProjectC.DTO;
using ProjectC.Service.Interface;
namespace ProjectC.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        
        private readonly IInvoiceService _service;

        private readonly IHostingEnvironment _hostingenviroment;

        public InvoiceController(IInvoiceService service, IHostingEnvironment hostingenvironment)
        {
            _service = service;
            _hostingenviroment = hostingenvironment;
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
        [HttpGet("ListAll/{id}")]
        public IActionResult ListAll(Guid id)
        {
            using (ProjectCContext ctx = new ProjectCContext())
            {

                var quqery = (from pd in ctx.Invoice.Where(x =>x.WorkOrderMasterId == id)
                         join od in ctx.InvoiceDetail on pd.InvoiceId equals od.InvoiceId 
                         into t
                         from rt in t.DefaultIfEmpty()
                         orderby pd.InvoiceId
                         select new
                         {
                             pd.SenderNo,
                             pd.SenderName,
                             pd.SenderAddress,
                             pd.SenderCity,
                             pd.SenderCountry,
                             pd.ConsgnName,
                             pd.ConsgnAddress,
                             pd.ConsgnCity,
                             pd.ConsgnCountry,
                             pd.TransptrName,
                             pd.VesselName,
                             pd.AgentName,
                             pd.PlateNo,
                             pd.AwbNo,
                             pd.Blno,
                             pd.Incoterms,
                             pd.DeliveryLocation,
                             pd.InvoiceAmount,
                             pd.InvoiceCurrency,
                             pd.FreightAmount,
                             pd.FreightCurrency,
                             pd.InsuranceAmount,
                             pd.InsuranceCurrency,
                             pd.CreatedDate,
                             rt.HsCode,
                             rt.DescGoods,
                             rt.ProductNo,
                             rt.CountryOfOrigin,
                             rt.Uom,
                             rt.ActualQuantity,
                             rt.InvoiceQuantity,
                             rt.GrossWeight,
                             rt.NetWeight,
                             rt.IntrnlAgmt,
                             rt.InvoiceNo,
                             rt.InvoiceDate,
                             rt.PkgType,
                             rt.CommclDesc,
                             rt.NumberOfPackages,
                             rt.RecordStatusId

                             
                             

                         }).ToList();


                return Ok(quqery);

            }
            

        }

    }
}