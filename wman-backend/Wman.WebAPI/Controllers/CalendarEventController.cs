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
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CalendarEventController : ControllerBase
    {
        ICalendarEventLogic calendarEvent;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="calendarEvent"></param>
        public CalendarEventController(ICalendarEventLogic calendarEvent)
        {
            this.calendarEvent = calendarEvent;
        }
        /// <summary>
        /// gets events of today
        /// </summary>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetCurrentDayEvents")]
        public async Task<ActionResult<IEnumerable<CalendarWorkEventDTO>>> GetCurrentDayEvents()
        {
            try
            {
                var events =await calendarEvent.GetCurrentDayEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// gets events from this week
        /// </summary>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetCurrentWeekEvents")]
        public async Task<ActionResult<IEnumerable<CalendarWorkEventDTO>>> GetCurrentWeekEvents()
        {
            try
            {
                var events =await calendarEvent.GetCurrentWeekEvents();
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        /// <summary>
        /// gets custom day events 
        /// </summary>
        /// <param name="day"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetDayEvents/{day}")]
        public async Task<ActionResult<IEnumerable<CalendarWorkEventDTO>>> GetDayEvents(int day)
        {
            try
            {
                var events =await calendarEvent.GetDayEvents(day);
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// gets custom week events
        /// </summary>
        /// <param name="week"></param>
        /// <returns>CalendarWorkEventDTO<returns>
        [HttpGet("GetWeekEvents/{week}")]
        public ActionResult<IEnumerable<CalendarWorkEventDTO>> GetWeekEvents(int week)
        {
            try
            {
                var events = calendarEvent.GetWeekEvents(week);
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// gets custom day events
        /// </summary>
        /// <param name="time"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetDayEvents")]
        public async Task<ActionResult<IEnumerable<CalendarWorkEventDTO>>> GetDayEvents(DateTime time)
        {
            try
            {
                var events = await calendarEvent.GetDayEvents(time);
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        /// <summary>
        /// gets custom week events
        /// </summary>
        /// <param name="startEventDate"></param>
        /// <param name="finishEventDate"></param>
        /// <returns>CalendarWorkEventDTO</returns>
        [HttpGet("GetWeekEvents")]
        public async Task<ActionResult<IEnumerable<CalendarWorkEventDTO>>> GetWeekEvents(DateTime startEventDate, DateTime finishEventDate)
        {
            try
            {
                var events = await calendarEvent.GetWeekEvents(startEventDate, finishEventDate);
                return Ok(events);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
    }
}
