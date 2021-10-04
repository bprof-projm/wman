using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.Interfaces;

namespace Wman.WebAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CalendarEventController : ControllerBase
    {
        ICalendarEventLogic calendarEvent;
        public CalendarEventController(ICalendarEventLogic calendarEvent)
        {
            this.calendarEvent = calendarEvent;
        }
        [HttpGet("GetCorrentDayEvents")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetCorrentDayEvents()
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
        [HttpGet("GetCorrentWeekEvents")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetCorrentWeekEvents()
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
        [HttpGet("GetDayEvents/{day}")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetDayEvents(int day)
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
        [HttpGet("GetWeekEvents/{week}")]
        public ActionResult<IEnumerable<WorkEvent>> GetWeekEvents(int week)
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
        [HttpGet("GetDayEvents")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetDayEvents(DateTime time)
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
        [HttpGet("GetWeekEvents")]
        public async Task<ActionResult<IEnumerable<WorkEvent>>> GetWeekEvents(DateTime startEventDate, DateTime finishEventDate)
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
