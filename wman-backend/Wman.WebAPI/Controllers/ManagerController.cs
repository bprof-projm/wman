using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Manager")]
    public class ManagerController : ControllerBase
    {
        IEventLogic eventLogic;
        IManagerLogic managerLogic;

        public ManagerController(IEventLogic eventLogic, IManagerLogic managerLogic)
        {
            this.eventLogic = eventLogic;
            this.managerLogic = managerLogic;

        }
        /// <summary>
        /// Endpoint to test the output of the upcoming xls export. Will be removed once excel exporting is implemented
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("testxls")]
        public async Task<ActionResult> test()
        {
            return Ok(await this.managerLogic.getStats());
        }
    }
}
