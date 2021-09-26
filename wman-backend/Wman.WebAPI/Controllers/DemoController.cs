using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wman.WebAPI.Controllers
{
    [ApiController]
    [Route("{controller}")]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public string HelloWorld()
        {
            return "Hello world";
        }
    }
}
