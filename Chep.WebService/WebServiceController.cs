using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Chep.WebService
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WebServiceController : ControllerBase
    {
        // GET: api/<WebServiceController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "aaa", "bbb" };
        }

        // POST api/<WebServiceController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
