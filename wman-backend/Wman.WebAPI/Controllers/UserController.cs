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
    [Authorize]
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
        /// Calculate the workload of the users with the usernames provided in post
        /// </summary>
        /// <param name="usernames">A collection of usernames</param>
        /// <returns>WorkLoadDTO</returns>
        [HttpPost]
        [Route("workload/range")]
        public async Task<ActionResult<IEnumerable<WorkloadDTO>>> workloadRange([FromBody] ICollection<string> usernames)
        {
            try
            {
                return Ok(await userLogic.getWorkLoads(usernames));
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return StatusCode(400, $"Error: {ex}");
                }
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// Calculate the workload of every single user
        /// </summary>
        /// <returns>WorkLoadDTO</returns>
        [HttpGet]
        [Route("workload")]
        public async Task<ActionResult<IEnumerable<WorkloadDTO>>> allWorkloads()
        {
            try
            {
                return Ok(await userLogic.getWorkLoads());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
    }
}
