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
    /// 
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        IAuthLogic authLogic;
        IAdminLogic adminLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authLogic"></param>
        /// <param name="adminLogic"></param>
        public AdminController(IAuthLogic authLogic, IAdminLogic adminLogic)
        {
            this.authLogic = authLogic;
            this.adminLogic = adminLogic;
        }
        /// <summary>
        /// Create a new worker
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateWorker([FromForm] RegisterDTO model)
        {
            await this.adminLogic.CreateWorker(model);
            return Ok();
        }
        /// <summary>
        /// Edit an existing worker
        /// </summary>
        /// <param name="username"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Modify/{username}")]
        public async Task<ActionResult> ModifyWorker(string username, [FromForm] WorkerModifyDTO model)
        {
            return Ok(await this.adminLogic.UpdateWorker(username, model));
        }
        /// <summary>
        /// Delete a worker
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeleteWorker(string username)
        {
            return Ok(await this.adminLogic.DeleteWorker(username));
        }
        /// <summary>
        /// Set the role of a user, while removing any previous roles it had before
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="rolename">Name of the role(Admin/Manager/Worker)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("setrole")]
        public async Task<ActionResult> SetRole(string username, string rolename)
        {
            await this.authLogic.SetRoleOfUser(username, rolename);
            return Ok();
        }
        
    }
}
