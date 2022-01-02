using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    /// <summary>
    /// EventController
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        IEventLogic eventLogic;
        IAuthLogic authLogic;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="eventLogic"></param>
        /// <param name="authLogic"></param>
        public EventController(IEventLogic eventLogic, IAuthLogic authLogic)
        {
            this.eventLogic = eventLogic;
            this.authLogic = authLogic;
        }
        /// <summary>
        /// Creates an event from body
        /// </summary>
        /// <param name="workEvent"></param>
        /// <returns></returns>
        [HttpPost("/CreateEvent")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> CreateEvent([FromBody] CreateEventDTO workEvent)
        {
                await eventLogic.CreateEvent(workEvent);
                return Ok();
        }

        /// <summary>
        /// deletes an event
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("/DeleteEvent/{Id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DeleteEvent(int Id)
        {
                await eventLogic.DeleteEvent(Id);
                return Ok();
        }

        [HttpPut("/DnDEvent/{id}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> DnDEvent(int id,[FromBody] DnDEventDTO workEvent)
        {
                await eventLogic.DnDEvent(id, workEvent);
                return Ok();
        }
        /// <summary>
        /// Assign a user to a specific event
        /// </summary>
        /// <param name="eventid">The id of the event</param>
        /// <param name="userName">username of the user</param>
        /// <returns>HTTP response code</returns>
        [HttpPost]
        [Route("assign")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> AssignUser(int eventid, string userName)
        {
                await eventLogic.AssignUser(eventid, userName);
                return Ok();

        }
#if DEBUG
        /// <summary>
        /// Debug endpoint used for listing every workevent. Will not be avaliable in production.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("all_debug")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetAll()
        {
            return Ok(await eventLogic.GetAllEvents());
        }
#endif
        /// <summary>
        /// Assign multiple users at a time to a selected event.
        /// </summary>
        /// <param name="eventid">The ID of the event we'd like to add to</param>
        /// <param name="usernames">A list of string string usernames, which we'd like to assign to the event</param>
        /// <returns>A list of userDTOs, where the users could be assigned without date collision</returns>
        [HttpPost]
        [Route("massAssign")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult> MassAssignUsers(int eventid, [FromBody] ICollection<string> usernames)
        {
                await eventLogic.MassAssignUser(eventid, usernames);
                return Ok();
        }

        [HttpPut("/UpdateEvent")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<UpdateEventDTO>> UpdateEvent([FromBody] UpdateEventDTO updateEvent) 
        {
                await eventLogic.UpdateEvent(updateEvent);
                return Ok();
        }
        [HttpPut("/StatusUpdater")]
        public async Task<ActionResult<WorkEventForWorkCardDTO>> UpdateEvent(int eventID)
        {
            var username = HttpContext.User.Identity.Name;
            var workevent = await eventLogic.StatusUpdater(eventID, username);
            return Ok(workevent);
        }
    }
}
