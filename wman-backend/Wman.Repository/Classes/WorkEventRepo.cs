using Microsoft.AspNetCore.Identity;
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
        UserManager<WmanUser> userManager;
        public WorkEventRepo(wmanDb inDb, UserManager<WmanUser> userManager)
        {
            this.db = inDb;
            this.userManager = userManager;
        }

        public async Task<int> AddEventReturnsId(WorkEvent element)
        {
            await this.db.AddAsync(element);
            await this.db.SaveChangesAsync();
            return element.Id;
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
            return this.db.WorkEvent.AsNoTracking().Include(x => x.Address).Include(x => x.AssignedUsers).ThenInclude(x => x.ProfilePicture).Include(x => x.Labels).Include(x => x.ProofOfWorkPic).OrderBy(x => x.EstimatedStartDate);
        }
        /// <inheritdoc />
        public async Task<WorkEvent> GetOne(int key) 
        {
            var entity = await (from x in db.WorkEvent
                          where x.Id == key
                          select x).AsNoTracking().Include(x => x.AssignedUsers).ThenInclude(x => x.ProfilePicture).Include(x => x.Address).Include(x => x.Labels).Include(x => x.ProofOfWorkPic).FirstOrDefaultAsync();
            return entity;
        }
        /// <inheritdoc />
        public async Task<WorkEvent> GetOneWithTracking(int key)
        {
            var entity = await (from x in db.WorkEvent
                                where x.Id == key
                                select x).Include(x => x.AssignedUsers).Include(x => x.ProofOfWorkPic).FirstOrDefaultAsync();
            return entity;
        }

        public async Task SaveDatabase()
        {
            await this.db.SaveChangesAsync();
        }

        public async Task Update(int oldKey, WorkEvent element)
        {
            var oldWorkEvent = await GetOne(oldKey);
            oldWorkEvent.JobDescription = element.JobDescription;
            oldWorkEvent.EstimatedStartDate = element.EstimatedStartDate;
            oldWorkEvent.EstimatedFinishDate = element.EstimatedFinishDate;
            oldWorkEvent.AssignedUsers = element.AssignedUsers;
            oldWorkEvent.Address = element.Address;
            await this.db.SaveChangesAsync();
        }
    }
}
