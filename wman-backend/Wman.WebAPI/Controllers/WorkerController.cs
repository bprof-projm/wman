using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager, Worker")]
    public class WorkerController : ControllerBase
    {
        IAllInWorkEventLogic allInWorkEvent;
        IUserLogic userLogic;

        public WorkerController(IAllInWorkEventLogic allInWorkEvent, IUserLogic userLogic)
        {
            this.userLogic = userLogic;
            this.allInWorkEvent = allInWorkEvent;
        }

        [HttpGet("/GetWorkersAvailablityAtSpecTime")]
        public async Task<ActionResult<List<WorkerDTO>>> GetWorkersAtSpecTime(DateTime fromDate, DateTime toDate)
        {
            var result = await allInWorkEvent.Available(fromDate, toDate);
            return Ok(result);
        }
        [HttpGet]
        [Route("asd")]
        public async Task<ActionResult> GetAvaliableEventsForLoggedIn()
        {
            var username = HttpContext.User.Identity.Name;
            //var username = User.FindFirst(ClaimTypes.Name).Value;
            
           
            return Ok(await userLogic.WorkEventsOfLoggedInUser(username));
        }

        [HttpGet]
        [Route("ASD2")]
        public async Task<ActionResult> GetEventDetailsForWorker(int id)
        {
            return Ok(userLogic.GetEventDetailsForWorker(HttpContext.User.Identity.Name, id));
        }
    }
}
