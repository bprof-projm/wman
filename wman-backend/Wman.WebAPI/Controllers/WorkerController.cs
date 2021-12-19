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
        /// Get all the events avaliable to the currently logged in user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("events/all")]
        public async Task<ActionResult> GetAvaliableEventsForMyself()
        {
            var username = HttpContext.User.Identity.Name;
            //var username = User.FindFirst(ClaimTypes.Name).Value;
            
           
            return Ok(await userLogic.WorkEventsOfLoggedInUser(username));
        }
        /// <summary>
        /// Lists all the workevents avaliable to the currently logged in user today
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("events/today")]
        public async Task<ActionResult> GetAvaliableEventsToday()
        {
            var username = HttpContext.User.Identity.Name;
            return Ok(await this.userLogic.WorkEventsOfUserToday(username));
        }
        /// <summary>
        /// Lists all the workevents avaliable to the currently logged in user this week
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("events/thisweek")]
        public async Task<ActionResult> GetAvaliableEventsThisWeek()
        {
            var username = HttpContext.User.Identity.Name;
            return Ok(await this.userLogic.WorkEventsOfUserThisWeek(username));
        }
        /// <summary>
        /// Lists all the workevents avaliable to the currently logged in user, within the specified time interval
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("events/date")]
        public async Task<ActionResult> GetAvaliableEventsSpecific(DateTime start, DateTime finish)
        {
            var username = HttpContext.User.Identity.Name;
            return Ok(await this.userLogic.WorkEventsOfUserSpecific(username, start, finish));
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
    }
}
