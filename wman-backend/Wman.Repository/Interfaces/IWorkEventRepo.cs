using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;

namespace Wman.Repository.Interfaces
{
    public interface IWorkEventRepo : IRepository<WorkEvent,int>
    {
        /// <summary>
        /// Get one workevent, without change tracking and address property
        /// Changes made to anything returned from this will not be tracked in db
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public new Task<WorkEvent> GetOne(int key);
        /// <summary>
        /// Get one workevent, WITH change tracking and address property
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Task<WorkEvent> GetOneWithTracking(int key);

        public Task<int> AddEventReturnsId(WorkEvent element);
    }
}
