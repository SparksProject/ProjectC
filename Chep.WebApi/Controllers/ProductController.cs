using System;
using System.Data;
using System.IO;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

//using Chep.Api.Models;
using Chep.DTO;
using Chep.Service.Interface;

//using Syncfusion.XlsIO;


namespace Chep.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }


        // CRUD
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

        [HttpPost("Add")]
        public IActionResult Post([FromBody] ProductDTO obj)
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
        public IActionResult Get(Guid id)
        {
            var result = _service.Get(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] ProductDTO obj)
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

        //[HttpPost("PostExcel")]
        //public IActionResult PostExcel([FromBody] ProductExcelModel obj)
        //{
        //    try
        //    {
        //        //obj.File = obj.File.Replace("data:application/pdf;base64", "");

        //        obj.File = obj.File.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", "");

        //        var decodedByteArray = Convert.FromBase64String(obj.File);

        //        string fileName = Guid.NewGuid() + "excel.xlsx";

        //        System.IO.File.WriteAllBytes(Path.Combine("Files", fileName), decodedByteArray);

        //        var datatable = ConvertExcelToDataTable(Path.Combine("Files", fileName));

        //        var result = _service.AddRange(datatable, obj.CustomerId, obj.CreatedBy);

        //        switch (result.ResultMessage)
        //        {
        //            case Enums.ResponseMessage.OK: return StatusCode(StatusCodes.Status200OK, result);
        //            case Enums.ResponseMessage.ERROR:
        //            case Enums.ResponseMessage.NOTFOUND:
        //            case Enums.ResponseMessage.UNAUTHORIZED:
        //            default: return StatusCode(StatusCodes.Status500InternalServerError, result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);
        //    }
        //}
        [HttpPost("PostExcel2")]
        public IActionResult PostExcel2(string Encode)
        {
            try
            {
                Encode = Encode.Replace("data:application/pdf;base64", "");

                var sPDFDecoded = Convert.FromBase64String(Encode);

                string fileName = Guid.NewGuid() + ".pdf";

                System.IO.File.WriteAllBytes(Path.Combine("Files", fileName), sPDFDecoded);



                //BinaryWriter writer = new BinaryWriter(File.Open(@"C:\Users\Administrator\Documents\pdf11.pdf", FileMode.CreateNew));
                //writer.Write(sPDFDecoded);



                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        // Privates
        //private DataTable ConvertExcelToDataTable(string path)
        //{
        //    using (Stream inputStream = System.IO.File.OpenRead(path))
        //    {
        //        using (ExcelEngine excelEngine = new ExcelEngine())
        //        {
        //            IApplication application = excelEngine.Excel;
        //            IWorkbook workbook = application.Workbooks.Open(inputStream);
        //            IWorksheet worksheet = workbook.Worksheets[0];

        //            DataTable dataTable = worksheet.ExportDataTable(worksheet.UsedRange, ExcelExportDataTableOptions.ColumnNames);
        //            return dataTable;
        //        }
        //    }
        //}
    }
}