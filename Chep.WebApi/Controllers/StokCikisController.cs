using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Chep.DTO;
using Chep.Service.Interface;

using System;
using System.Collections.Generic;

namespace Chep.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StokCikisController : Controller
    {
        private readonly IStokCikisService _service;
        private readonly Service.WorkOrderService _workOrderService;
        private readonly IStokGirisService _serviceGiris;

        public StokCikisController(IStokCikisService service, IStokGirisService serviceGiris)
        {
            _service = service;
            _serviceGiris = serviceGiris;
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

        [HttpGet("GetStokDusumListe")]
        public IActionResult GetStokDusumListe([FromQuery] string itemNo, [FromQuery] int cikisAdet)
        {
            try
            {
                var result = _service.GetStokDusumListe(itemNo, cikisAdet);

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
        public IActionResult InsertStokCikisFromStokDusumListe([FromQuery] int stokCikisId, [FromQuery] string itemNo, [FromQuery] int cikisAdet)
        {
            try
            {
                var stokDusumListeResult = _service.GetStokDusumListe(itemNo, cikisAdet);

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
        public IActionResult SetWorkOrderService(int id)
        {
            var result = _workOrderService.SetWorkOrderMastersModel(id);

            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}