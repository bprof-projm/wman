using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    /// <summary>
    /// Auth controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]

    public class AuthController : Controller
    {
        IAuthLogic authLogic;
        IAdminLogic adminLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authLogic"></param>
        /// <param name="adminLogic"></param>
        public AuthController(IAuthLogic authLogic, IAdminLogic adminLogic)
        {
            this.authLogic = authLogic;
            this.adminLogic = adminLogic;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model">Login details</param>
        [HttpPut]
        [Route("login")]

        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {

            return Ok(await authLogic.LoginUser(model));
        }

        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("all")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            return Ok(await authLogic.GetAllUsers());

        }

        /// <summary>
        /// Get one specific user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpGet("username")]
        //[Route("getOne")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<UserDTO>> GetUser(string username)
        {
            return Ok(await authLogic.GetOneUser(username));
        }

        /// <summary>
        /// Returns a list of users that have the provided role
        /// </summary>
        /// <param name="rolename">Name of the role</param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/members")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> UsersOfRole(string rolename)
        {
            return Ok(await this.authLogic.GetAllUsersOfRole(rolename));
        }
        /// <summary>
        /// Returns the role(s) assigned to the user. (Worker/Admin/Manager)
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Worker/Admin/Manager</returns>
        [HttpGet]
        [Route("userrole")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<string>>> RolesOfUser(string username)
        {
            return Ok(await this.authLogic.GetAllRolesOfUser(username));
        }
    }
}
