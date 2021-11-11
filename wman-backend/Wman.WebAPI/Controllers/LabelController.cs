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
    [Authorize(Roles = "Manager")]
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
                await labelLogic.CreateLabel(label);
                return Ok();
        }

        [HttpGet("/GetAllLabel")]
        public ActionResult<ListLabelsDTO> GetAllLabel()
        {
                return Ok(labelLogic.GetAllLabels());
        }

        [HttpPut("/UpdateLabel/{Id}")]
        public async Task<ActionResult> UpdateLabel(int Id, [FromBody] CreateLabelDTO newLabel)
        {
                await labelLogic.UpdateLabel(Id, newLabel);
                return Ok();
        }
        [HttpPost("/AssignLabelToWorkEvent")]
        public async Task<ActionResult> AssignLabelToWorkEvent(int eventId, int labelId)
        {
                await labelLogic.AssignLabelToWorkEvent(eventId, labelId);
                return Ok();
        }
        [HttpDelete("/DeleteLabel/{Id}")]
        public async Task<ActionResult> DeleteLabel(int Id)
        {
                await labelLogic.DeleteLabel(Id);
                return Ok();
        }
    }
}
