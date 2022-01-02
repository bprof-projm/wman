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
    }
}
