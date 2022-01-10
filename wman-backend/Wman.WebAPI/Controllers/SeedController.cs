using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Logic.Helpers;

namespace Wman.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "SystemAdmin")]
    public class SeedController : ControllerBase
    {
        DBSeed dBSeed;

        public SeedController(DBSeed seed)
        {
            dBSeed = seed;
        }

        /// <summary>
        /// Fill database with testing data. Used only for testing purposes.
        /// </summary>
        /// <returns>200</returns>
        [HttpGet]
        public async Task<ActionResult> PopulateDB()
        {
            dBSeed.PopulateDB();
            return Ok();
        }
    }
}
