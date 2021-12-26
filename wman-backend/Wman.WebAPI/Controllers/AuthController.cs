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
        DBSeed dBSeed;
        IAdminLogic adminLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authLogic"></param>
        /// /// <param name="dBSeed"></param>
        /// <param name="adminLogic"></param>
        public AuthController(IAuthLogic authLogic, DBSeed dBSeed, IAdminLogic adminLogic)
        {
            this.authLogic = authLogic;
            this.dBSeed = dBSeed;
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

        /// <summary>
        /// Api used to create a new admin user, if there are none present.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AdminSetup")]
        public async Task<ActionResult> FirstTimeSetup([FromForm] RegisterDTO model)
        {
            await adminLogic.Setup(model);
            return Ok();
        }

        /// <summary>
        /// DEBUG Endpoint used to fill database with testing data. Used only for development purposes.
        /// </summary>
        /// <returns>200</returns>
        [HttpGet]
        [Route("db")]

        public async Task<ActionResult> PopulateDB()
        {
            dBSeed.PopulateDB();
            return Ok();
        }
    }
}
