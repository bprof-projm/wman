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
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager, Worker")]
    public class WorkerController : ControllerBase
    {
        IAllInWorkEventLogic allInWorkEvent;

        public WorkerController(IAllInWorkEventLogic allInWorkEvent)
        {
            this.allInWorkEvent = allInWorkEvent;
        }

        [HttpGet("/GetWorkersAvailablityAtSpecTime")]
        public async Task<ActionResult<List<WorkerDTO>>> GetWorkersAtSpecTime(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = await allInWorkEvent.Available(fromDate, toDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
    }
}
