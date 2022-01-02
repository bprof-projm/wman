using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager, Worker")]
    public class WorkerController : ControllerBase
    {
        IAllInWorkEventLogic allInWorkEvent;
        IUserLogic userLogic;

        public WorkerController(IAllInWorkEventLogic allInWorkEvent, IUserLogic userLogic)
        {
            this.userLogic = userLogic;
            this.allInWorkEvent = allInWorkEvent;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        [HttpGet("/GetWorkersAvailablityAtSpecTime")]
        public async Task<ActionResult<List<WorkerDTO>>> GetWorkersAtSpecTime(DateTime fromDate, DateTime toDate)
        {
            var result = await allInWorkEvent.Available(fromDate, toDate);
            return Ok(result);
        }
        
        /// <summary>
        /// Get event details (Only works on the ones assigned to the currently logged in user)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("eventdetail")]
        public async Task<ActionResult> GetEventDetailsForMyself(int id)
        {
            return Ok(await userLogic.GetEventDetailsForWorker(HttpContext.User.Identity.Name, id));
        }
        /// <summary>
        /// Return the workload of the currently logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("myworkload")]
        public async Task<ActionResult> GetLoggedInWorkersWorkload()
        {
            var templist = new List<string>() { HttpContext.User.Identity.Name };
            return Ok(await userLogic.GetWorkLoads(templist));
        }
    }
}
