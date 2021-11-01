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
    public class LabelController : ControllerBase
    {
        ILabelLogic labelLogic;
        public LabelController(ILabelLogic labelLogic)
        {
            this.labelLogic = labelLogic;
        }

        [HttpPost("/CreateLabel")]
        public async Task<ActionResult> CreateLabel([FromBody] CreateLabelDTO label)
        {
            try
            {
                await labelLogic.CreateLabel(label);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        [HttpGet("/GetAllLabel")]
        public ActionResult<ListLabelsDTO> GetAllLabel()
        {
            try
            {
                
                return Ok(labelLogic.GetAllLabels());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }

        [HttpPut("/UpdateLabel/{Id}")]
        public async Task<ActionResult> UpdateLabel(int Id, [FromBody] CreateLabelDTO newLabel)
        {
            try
            {
                await labelLogic.UpdateLabel(Id, newLabel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        [HttpPost("/AssignLabelToWorkEvent")]
        public async Task<ActionResult> AssignLabelToWorkEvent(int eventId, int labelId)
        {
            try
            {
                await labelLogic.AssignLabelToWorkEvent(eventId, labelId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
        [HttpDelete("/DeleteLabel/{Id}")]
        public async Task<ActionResult> DeleteLabel(int Id)
        {
            try
            {
                await labelLogic.DeleteLabel(Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error : {ex}");
            }
        }
    }
}
