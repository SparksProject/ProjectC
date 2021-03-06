using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chep.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StokCikisController : Controller
    {
        private readonly IStokCikisService _service;
        private readonly IWorkOrderService _workOrderService;

        public StokCikisController(IStokCikisService service, IWorkOrderService workOrderService)
        {
            _service = service;
            _workOrderService = workOrderService;
        }


        // Crud
        [HttpGet("List")]
        public IActionResult List([FromQuery] int? referansNo, [FromQuery] string beyannameNo, [FromQuery] string siparisNo)
        {
            var result = _service.List(referansNo, beyannameNo, siparisNo);

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

        [HttpPost("WorkOrderStatusEdit")]
        public IActionResult WorkOrderStatusEdit([FromBody] ChepStokCikisDTO obj)
        {

            var result = _service.WorkOrderStatusEdit(obj);

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
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
                default:
                    return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpGet("GetStokDusumListe")]
        public IActionResult GetStokDusumListe([FromQuery] string itemNo, [FromQuery] int cikisAdet, [FromQuery] Guid ithalatciFirma)
        {
            try
            {
                var result = _service.GetStokDusumListe(itemNo, cikisAdet, ithalatciFirma);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost("StokDusumListeAdd")]
        public IActionResult StokDusumListeAdd([FromBody] ChepStokCikisDTO obj)
        {
            try
            {
                var result = _service.StokDusumListeAdd(obj.ItemNo, obj.DropCount, obj.ChepStokCikisDetayList);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("InsertStokCikisFromStokDusumListe")]
        public IActionResult InsertStokCikisFromStokDusumListe([FromQuery] int stokCikisId, [FromQuery] string itemNo, [FromQuery] int cikisAdet, [FromQuery] Guid customerId)
        {
            try
            {
                var stokDusumListeResult = _service.GetStokDusumListe(itemNo, cikisAdet, customerId);

                if (!stokDusumListeResult.IsSuccesful || stokDusumListeResult.Result == null)
                {
                    return NotFound();
                }

                var target = _service.AddDetail(stokCikisId, stokDusumListeResult.Result as List<ViewStokDusumListeDto>);

                return StatusCode(StatusCodes.Status200OK, target);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpGet("SetWorkOrderService/{id}")]
        public async Task<IActionResult> SetWorkOrderService(int id)
        {
            var result = await _workOrderService.SetWorkOrderMastersModel(id);

            return StatusCode(StatusCodes.Status200OK, result);
        }


        [HttpGet("GetByUrunKod/{id}")]
        public IActionResult GetByUrunKod(string id)
        {
            var result = _service.GetByUrunKod(id).Result;

            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}