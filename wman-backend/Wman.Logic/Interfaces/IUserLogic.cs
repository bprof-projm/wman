using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IUserLogic
    {
        public Task<IEnumerable<WorkloadDTO>> getWorkLoads(IEnumerable<string> usernames);
        public Task<IEnumerable<WorkloadDTO>> getWorkLoads();

        Task<IEnumerable<AssignedEventDTO>> workEventsOfUser(string username);
    }
}
