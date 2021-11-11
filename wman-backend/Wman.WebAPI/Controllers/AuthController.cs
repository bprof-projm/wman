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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authLogic"></param>
        /// /// <param name="dBSeed"></param>
        public AuthController(IAuthLogic authLogic, DBSeed dBSeed)
        {
            this.authLogic = authLogic;
            this.dBSeed = dBSeed;
        }

        /// <summary>
        /// Login/generate jwt token
        /// </summary>
        /// <param name="model">Login details</param>
        /// <returns>Hopefully a jwt token</returns>
        [HttpPut]
        [Route("login")]

        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {

            return Ok(await authLogic.LoginUser(model));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model">Login model</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateWorker([FromBody] RegisterDTO model)
        {
            return Ok("User created successfully");
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
            return Ok(authLogic.GetOneUser(username));
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="username">Username of the user to be deleted</param>
        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            return Ok(await this.authLogic.DeleteUser(username));
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="oldUsername">Prev. id</param>
        /// <param name="pwd">Password of the user to be updated</param>
        /// <param name="user">User to be updated</param>
        [HttpPut("{oldUsername}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateUser(string oldUsername, string pwd, [FromBody] UserDTO user)
        {
            return Ok(await this.authLogic.UpdateUser(oldUsername, pwd, user));
        }

        /// <summary>
        /// Set the role of a user, while removing any previous roles he had before
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="rolename">Name of the role(Admin/Manager/Worker)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/set")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> SetRole(string username, string rolename)
        {
            await this.authLogic.SetRoleOfUser(username, rolename);
            return Ok();
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
        /// Endpoint used to fill database with testing data. Used only for development purposes.
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
