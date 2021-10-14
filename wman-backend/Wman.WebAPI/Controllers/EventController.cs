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
    //[Authorize]
    public class EventController : ControllerBase
    {
        IEventLogic eventLogic;
        IAuthLogic authLogic;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="eventLogic"></param>
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
        public async Task<ActionResult> CreateEvent([FromBody] CreateEventDTO workEvent)
        {
            try
            {
                await eventLogic.CreateEvent(workEvent);
                return Ok();
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// Getting a custom event back
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("/GetEvent/{Id}")]
        public async Task<ActionResult<CreateEventDTO>> GetEvent(int Id)
        {
            try
            {
                var entity = await eventLogic.GetEvent(Id);
                return Ok(entity);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// Get all the events
        /// </summary>
        /// <returns>A collection of all the events</returns>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<IEnumerable<CreateEventDTO>>> GetAll()
        {
            try
            {
                var output = await eventLogic.GetAllEvents();
                return Ok(output);
            }
            catch (Exception ex)
            {

                return StatusCode(500, ex);
            }
        }

        /// <summary>
        /// deletes an event
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("/DeleteEvent/{Id}")]
        public async Task<ActionResult> DeleteEvent(int Id)
        {
            try
            {
                await eventLogic.DeleteEvent(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// Assign a user to a specific event
        /// </summary>
        /// <param name="eventid">The id of the event</param>
        /// <param name="userName">username of the user</param>
        /// <returns>HTTP response code</returns>
        [HttpPost]
        [Route("assign")]
        public async Task<ActionResult> AssignUser(int eventid, string userName)
        {
            try
            {
                await eventLogic.AssignUser(eventid, userName);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return StatusCode(400, $"Error : {ex}");
                }
                return StatusCode(500, $"Internal server error : {ex}");
            }

        }

        /// <summary>
        /// Assign multiple users at a time to a selected event.
        /// </summary>
        /// <param name="eventid">The ID of the event we'd like to add to</param>
        /// <param name="usernames">A list of string string usernames, which we'd like to assign to the event</param>
        /// <returns>A list of userDTOs, where the users could be assigned without date collision</returns>
        [HttpPost]
        [Route("massAssign")]
        public async Task<ActionResult<ICollection<UserDTO>>> MassAssignUsers(int eventid, [FromBody] ICollection<string> usernames)
        {
            try
            {
                return Ok(await eventLogic.MassAssignUser(eventid, usernames));
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    return StatusCode(400, $"Error : {ex}");
                }
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// Lists all the users assigned to a selected event
        /// </summary>
        /// <param name="eventid">The ID of the event</param>
        /// <returns>HTTP response code</returns>
        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<ICollection<UserDTO>>> GetAllAssignedUsers(int eventid)
        {
            return Ok(await eventLogic.GetAllAssignedUsers(eventid));
        }


    }
}
