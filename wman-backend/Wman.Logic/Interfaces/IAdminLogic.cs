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
        Task<IdentityResult> CreateWorker(RegisterDTO login);

        Task<IdentityResult> DeleteWorker(string uname);

        Task<IdentityResult> UpdateWorker(string username, WorkerModifyDTO model);
        void test(IAuthLogic logic);
    }
}
