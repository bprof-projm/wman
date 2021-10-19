using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Repository.Classes
{
    public class WorkEventRepo : IWorkEventRepo
    {
        private wmanDb db;
        public WorkEventRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public async Task Add(WorkEvent element)
        {
            await this.db.AddAsync(element);
            await this.db.SaveChangesAsync();
        }

        public async Task Delete(int element)
        {
            var entity =await GetOne(element);
            this.db.Remove(entity);
            await this.db.SaveChangesAsync();
        }

        public IQueryable<WorkEvent> GetAll()
        {
            return this.db.WorkEvent.AsNoTracking().Include(x => x.Address);
        }

        public async Task<WorkEvent> GetOne(int key)
        {
            var entity = await (from x in db.WorkEvent
                          where x.Id == key
                          select x).Include(x => x.AssignedUsers).AsNoTracking().Include(x => x.Address).FirstOrDefaultAsync();
            return entity;
        }
        public async Task Update(int oldKey, WorkEvent element)
        {
            var oldWorkEvent = await GetOne(oldKey);
            oldWorkEvent.JobDescription = element.JobDescription;
            oldWorkEvent.EstimatedStartDate = element.EstimatedStartDate;
            oldWorkEvent.EstimatedFinishDate = element.EstimatedFinishDate;
            oldWorkEvent.AssignedUsers = element.AssignedUsers;
            await this.db.SaveChangesAsync();
        }
    }
}
