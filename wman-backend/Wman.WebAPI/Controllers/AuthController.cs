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
        IAuthLogic authLogic;

        public AuthController(IAuthLogic authLogic)
        {
            this.authLogic = authLogic;
        }
        [HttpPost]
        public async Task<ActionResult> CreateUser([FromBody] Login model)
        {
            string result = await authLogic.CreateUser(model);
            return Ok(new { UserName = result });
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<WmanUser> GetAllUsers()
        {
            return authLogic.GetAllUsers();
        }


        [HttpGet]
        [Route("getOne")]
        [Authorize(Roles = "Admin")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async void DeleteUser(int id)
        {
            await this.authLogic.DeleteUser(id);
        }

        [HttpPut("{oldId}")]
        [Authorize(Roles = "Admin")]
        public async void UpdateUser(int oldId, [FromBody] WmanUser user)
        {
            await this.authLogic.UpdateUser(oldId, user);
        }

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
    }
}
