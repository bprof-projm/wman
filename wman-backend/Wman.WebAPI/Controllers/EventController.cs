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
                var entity =await eventLogic.GetEvent(Id);
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
                var output = eventLogic.GetAllEvents();
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

        [HttpPost]
        [Route("assign")]
        public async Task<ActionResult> AssignUser(int eventid, string userName)
        {
            var selectedUser = await authLogic.GetOneUser(userName);
            await eventLogic.AssignUser(eventid, selectedUser);
            return Ok();
        }

        [HttpGet]
        [Route("users")]
        public async Task<ActionResult<ICollection<WmanUser>>> GetAllAssignedUsers(int eventid)
        {
            var selectedEvent = await eventLogic.GetEvent(eventid);
            ;
            return Ok(selectedEvent.AssignedUsers);
            throw new NotImplementedException();
        }
    }
}
