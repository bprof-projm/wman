using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface ILabelLogic
    {
        Task CreateLabel(CreateLabelDTO label);
        List<ListLabelsDTO> GetAllLabels();
        Task UpdateLabel(int Id, CreateLabelDTO NewLabel);
        Task AssignLabelToWorkEvent(int eventId, int labelId);
        Task DeleteLabel(int Id);
    }
}
