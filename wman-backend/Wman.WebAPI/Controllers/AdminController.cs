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
        /// Create a new workforce
        /// </summary>
        /// <param name="model"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> CreateWorkforce([FromForm] RegisterDTO model, string role)
        {
            await this.adminLogic.CreateWorkforce(model, role);
            return Ok();
        }
        /// <summary>
        /// Edit an existing workforce
        /// </summary>
        /// <param name="username"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Modify/{username}")]
        public async Task<ActionResult> ModifyWorkForce(string username, [FromForm] WorkerModifyDTO model)
        {
            return Ok(await this.adminLogic.UpdateWorkforce(username, model));
        }
        /// <summary>
        /// Delete a workforce
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Delete")]
        public async Task<ActionResult> DeleteWorkforce(string username)
        {
            return Ok(await this.adminLogic.DeleteWorkforce(username));
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
