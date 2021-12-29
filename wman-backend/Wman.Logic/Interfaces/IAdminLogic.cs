using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IAdminLogic
    {
        Task<IdentityResult> CreateWorkforce(RegisterDTO regdto);

        Task<IdentityResult> DeleteWorkforce(string uname);

        Task<IdentityResult> UpdateWorkforce(string username, WorkerModifyDTO model);
    }
}
