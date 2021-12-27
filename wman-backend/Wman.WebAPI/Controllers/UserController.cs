using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    /// <summary>
    /// UserController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager")]
    public class UserController : ControllerBase
    {
        IUserLogic userLogic;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="userLogic"></param>
        public UserController(IUserLogic userLogic)
        {
            this.userLogic = userLogic;
        }

        /// <summary>
        /// Calculate the workload of the users with the usernames provided in query
        /// </summary>
        /// <param name="usernames">A collection of usernames</param>
        /// <returns>WorkLoadDTO</returns>
        [HttpGet]
        [Route("workload/range")]
        public async Task<ActionResult<IEnumerable<WorkloadDTO>>> WorkloadRange([FromQuery] ICollection<string> usernames)
        {
            return Ok(await userLogic.GetWorkLoads(usernames));
        }

        /// <summary>
        /// Calculate the workload of every single user
        /// </summary>
        /// <returns>WorkLoadDTO</returns>
        [HttpGet]
        [Route("workload")]
        public async Task<ActionResult<IEnumerable<WorkloadDTO>>> AllWorkloads()
        {
            return Ok(await userLogic.GetWorkLoads());
        }


        /// <summary>
        /// Get all the events to which a selected user is assigned to
        /// </summary>
        /// <param name="username">Username of the searched user</param>
        /// <returns>A collection of events that are assigned to the specified user</returns>
        [HttpGet]
        [Route("workEvents")]
        public async Task<ActionResult<ICollection<AssignedEventDTO>>> GetAssignedEventsOfUser(string username)
        {
            var result = await userLogic.WorkEventsOfUser(username);
            return Ok(result);
        }
        /// <summary>
        /// Calculate the workload stats of a selected user, in the specified year
        /// </summary>
        /// <param name="username"></param>
        /// <param name="date">Datetime, containing the year we're filtering to</param>
        /// <returns></returns>
        [HttpGet]
        [Route("stats")]
        public async Task<ActionResult> CalculateStats(string username, DateTime date)
        {
            return Ok(await this.userLogic.GetMonthlyStats(username, date));
        }
    }
}
