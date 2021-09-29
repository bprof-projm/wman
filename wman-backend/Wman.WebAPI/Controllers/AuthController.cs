using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Classes;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        AuthLogic authLogic;

        public AuthController(AuthLogic authLogic)
        {
            this.authLogic = authLogic;
        }
        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model">Login model</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] Login model)
        {
            string result = await authLogic.CreateUser(model);
            return Ok(new { UserName = result });
        }
        /// <summary>
        /// Get a list of all users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public IEnumerable<WmanUser> GetAllUsers()
        {
            return authLogic.GetAllUsers();
        }

        /// <summary>
        /// Get one specific user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOne")]
        //[Authorize(Roles = "Admin")]
        public WmanUser GetUser([FromQuery] string id)
        {
            if (id.Contains('@'))
            {
                return this.authLogic.GetOneUser(-1, id);
            }
            else
            {
                return this.authLogic.GetOneUser(int.Parse(id), null);
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">Id of the user to be deleted</param>
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")]
        public async void DeleteUser(int id)
        {
            await this.authLogic.DeleteUser(id);
        }

        /// <summary>
        /// Update a user
        /// </summary>
        /// <param name="oldId">Prev. id</param>
        /// <param name="user">User to be updated</param>
        [HttpPut("{oldId}")]
        //[Authorize(Roles = "Admin")]
        public async void UpdateUser(int oldId, [FromBody] WmanUser user)
        {
            await this.authLogic.UpdateUser(oldId, user);
        }

        /// <summary>
        /// Login/generate jwt token
        /// </summary>
        /// <param name="model">Login details</param>
        /// <returns>Hopefully a jwt token</returns>
        [HttpPut]
        public async Task<ActionResult> Login([FromBody] Login model)
        {
            try
            {
                return Ok(await authLogic.LoginUser(model));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        /// <summary>
        /// debug endpoint
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("test")]
        public async Task<ActionResult> debugreg()
        {
            string result = await authLogic.CreateUser(new Login() { Email = "luxederzoltan@gmail.com", Password = "asdf123"});
            return Ok(new { UserName = result });
        }
    }
}
