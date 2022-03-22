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
        /// Endpoint to test the output of the upcoming xls export. Will be removed once excel exporting is implemented
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("GenerateXls")]
        public async Task<ActionResult> StatsThisMonth()
        {
            return Ok(await this.statsLogic.GetStats(DateTime.Now));
        }

        /// <summary>
        /// Endpoint used for sending the xls stats to all the managers. 
        /// </summary>
        /// <param name="filename">Name of the .xlsx. If left empty, the latest one is used</param>
        /// <returns></returns>
        [HttpGet("sendemails")]
        public async Task<ActionResult> SendEmails(string filename)
            {
            await this.statsLogic.SendEmails(filename);
            return Ok();
        }
    }
}
