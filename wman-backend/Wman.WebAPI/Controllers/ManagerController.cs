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
    [Authorize(Roles = "Manager, Admin")]
    public class ManagerController : ControllerBase
    {
        IEventLogic eventLogic;
        IStatsLogic statsLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventLogic"></param>
        /// <param name="statsLogic"></param>
        public ManagerController(IEventLogic eventLogic, IStatsLogic statsLogic)
        {
            this.eventLogic = eventLogic;
            this.statsLogic = statsLogic;

        }
        /// <summary>
        /// Testing endpoint to test the output of the xls export.
        /// </summary>
        /// <returns></returns>
        /// 
        [Authorize(Roles = "Admin")]
        [HttpGet("GenerateXls")]
        public async Task<ActionResult> StatsThisMonth()
        {
            return Ok(await this.statsLogic.GetManagerStats(DateTime.Now));
        }

        /// <summary>
        /// Testing endpoint used for sending the xls stats to all the managers. 
        /// </summary>
        /// <param name="filename">Name of the .xlsx. If left empty, the latest one is used</param>
        /// <returns></returns>
        [HttpGet("Sendemails")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SendEmails(string filename)
            {
            await this.statsLogic.SendManagerEmails(filename);
            return Ok();
        }
    }
}
