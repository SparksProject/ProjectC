using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Chep.DTO;
using Chep.Service.Interface;
using Microsoft.AspNetCore.Http;
//using Chep.Api.Models;

namespace Chep.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("IsAuthenticated")]
        public IActionResult IsAuthenticated()
        {
            var token = Request.Headers["Authorization"];

            if (_userService.ValidateToken(token))
            {
                return StatusCode(StatusCodes.Status200OK, "OK");
            }
            else
            {
                return StatusCode(StatusCodes.Status401Unauthorized, "Unauthorized");
            }
        }

        [HttpPost("Authenticate")]
        public IActionResult Authenticate([FromBody] UserDTO model)
        {
            var result = _userService.Authenticate(model.UserName, model.Password);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
                case Enums.ResponseMessage.NOTFOUND:
                    return StatusCode(StatusCodes.Status404NotFound);
                case Enums.ResponseMessage.ERROR:
                    return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
                case Enums.ResponseMessage.UNAUTHORIZED:
                    return StatusCode(StatusCodes.Status401Unauthorized);
                default:
                    return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("Edit")]
        public IActionResult Edit([FromBody] UserDTO obj)
        {
            var result = _userService.Edit(obj);

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

        //[HttpGet("Get/{id}")]
        //public IActionResult Get(string id)
        //{
        //    var token = Request.Headers["Authorization"];

        //    var result = _userService.Get(token, id);

        //    switch (result.ResultMessage)
        //    {
        //        case Enums.ResponseMessage.OK:
        //            return StatusCode(StatusCodes.Status200OK, result.Result);
        //        case Enums.ResponseMessage.ERROR:
        //            return StatusCode(StatusCodes.Status500InternalServerError, result.Exception);
        //        case Enums.ResponseMessage.NOTFOUND:
        //            return StatusCode(StatusCodes.Status404NotFound);
        //        case Enums.ResponseMessage.UNAUTHORIZED:
        //            return StatusCode(StatusCodes.Status401Unauthorized);
        //        default:
        //            return StatusCode(StatusCodes.Status404NotFound);
        //    }
        //}

        [HttpGet("GetUser/{id}")]
        public IActionResult GetUser(int id)
        {
            var result = _userService.Get(id);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
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

        [HttpGet("List")]
        public IActionResult List()
        {
            var result = _userService.List();

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
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
        public IActionResult Add([FromBody] UserDTO obj)
        {
            var result = _userService.Add(obj);

            switch (result.ResultMessage)
            {
                case Enums.ResponseMessage.OK:
                    return StatusCode(StatusCodes.Status200OK, result);
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