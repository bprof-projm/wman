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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                return Ok(await authLogic.LoginUser(model));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model">Login model</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CreateUser([FromBody] RegisterDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result;
            try
            {
                result = await authLogic.CreateUser(model);

                if (result.Succeeded) return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return UnprocessableEntity(new { Error = ex.Message });
                throw;
            }
            return BadRequest(result.Errors);
        }
        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllUsers()
        {
            try
            {
                return Ok(Converter.MassConvert(await authLogic.GetAllUsers()));
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }

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
            var output = Converter.Convert(await authLogic.GetOneUser(username));
            if (output == null)
            {
                return BadRequest("User not found");
            }
            return Ok(output);
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="username">Username of the user to be deleted</param>
        [HttpDelete("{username}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            IdentityResult result;
            try
            {
                result = await this.authLogic.DeleteUser(username);
                if (result.Succeeded)
                    return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {

                return BadRequest(new { Error = ex.Message });
            }
            return BadRequest(result.Errors);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            IdentityResult result;
            try
            {
                result = await this.authLogic.UpdateUser(oldUsername, pwd, user);
                if (result.Succeeded)
                    return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Set the role of a user, while removing any previous roles he had before
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="rolename">Name of the role(Admin/Manager/Worker)</param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/assign")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignRole(string username, string rolename)
        {
            try
            {
                await this.authLogic.SetRoleOfUser(username, rolename);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
            return Ok();
        }

        /// <summary>
        /// Returns a list of rolenames the selected user is assigned to.
        /// </summary>
        /// <param name="username">username of the user</param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/userroles")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<string>>> RolesOfUser(string username)
        {
            try
            {
                return Ok(await this.authLogic.GetAllRolesOfUser(username));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// Returns a list of users that have the provided role
        /// </summary>
        /// <param name="rolename">Name of the rule</param>
        /// <returns></returns>
        [HttpGet]
        [Route("role/members")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> UsersOfRole(string rolename)
        {
            try
            {
                return Ok(await this.authLogic.GetAllUsersOfRole(rolename));
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// Endpoint used to fill database with testing data. Used only for development purposes.
        /// </summary>
        /// <returns>200</returns>
        [HttpGet]
        [Route("db")]

        public async Task<ActionResult> PopulateDB()
        {
            try
            {
                dBSeed.PopulateDB();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
            return Ok();
        }
    }
}
