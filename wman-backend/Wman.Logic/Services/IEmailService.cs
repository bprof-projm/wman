using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Logic.Services
{
    public interface IEmailService
    {
        Task AssigedToWorkEvent(WorkEvent we, WmanUser user);
        Task WorkEventUpdated(WorkEvent we, WmanUser user);
        Task SendXls(WmanUser user, string path);
    }
}
