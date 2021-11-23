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
    /// CalendarEvent controller
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, Manager, Worker")]
    public class CalendarEventController : ControllerBase
    {
        ICalendarEventLogic calendarEvent;
        IAllInWorkEventLogic workCardEvent;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="calendarEvent"></param>
        public CalendarEventController(ICalendarEventLogic calendarEvent, IAllInWorkEventLogic workCardEvent)
        {
            this.calendarEvent = calendarEvent;
            this.workCardEvent = workCardEvent;
        }
        /// <summary>
        /// gets events of today
        /// </summary>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetCurrentDayEvents")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetCurrentDayEvents()
        {
                var events =await calendarEvent.GetCurrentDayEvents();
                return Ok(events);
        }
        /// <summary>
        /// gets events from this week
        /// </summary>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetCurrentWeekEvents")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetCurrentWeekEvents()
        {
                var events =await calendarEvent.GetCurrentWeekEvents();
                return Ok(events);
        }

        /// <summary>
        /// gets custom day events 
        /// </summary>
        /// <param name="day"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetDayEvents/{day}")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetDayEvents(int day)
        {
                var events =await calendarEvent.GetDayEvents(day);
                return Ok(events);
        }
        /// <summary>
        /// gets custom week events
        /// </summary>
        /// <param name="week"></param>
        /// <returns>CalendarWorkEventDTO<returns>
        [HttpGet("GetWeekEvents/{week}")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetWeekEvents(int week)
        {
                var events = await calendarEvent.GetWeekEvents(week);
                return Ok(events);
        }
        /// <summary>
        /// gets custom day events
        /// </summary>
        /// <param name="time"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetDayEvents")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetDayEvents(DateTime time)
        {
                var events = await calendarEvent.GetDayEvents(time);
                return Ok(events);
        }
        /// <summary>
        /// gets custom week events
        /// </summary>
        /// <param name="startEventDate"></param>
        /// <param name="finishEventDate"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetWeekEvents")]
        public async Task<ActionResult<IEnumerable<WorkEventForWorkCardDTO>>> GetWeekEvents(DateTime startEventDate, DateTime finishEventDate)
        {
                var events = await calendarEvent.GetWeekEvents(startEventDate, finishEventDate);
                return Ok(events);
        }

        [HttpGet("WorkCard/{Id}")]
        public async Task<ActionResult<WorkEventForWorkCardDTO>> ForWorkCard(int Id)
        {
                var workCard = await workCardEvent.ForWorkCard(Id);
                return Ok(workCard);
        }
    }
}
