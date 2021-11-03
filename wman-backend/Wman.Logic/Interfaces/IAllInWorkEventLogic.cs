using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IAllInWorkEventLogic
    {
       Task<WorkEventForWorkCardDTO> ForWorkCard(int eventId);
    }
}
