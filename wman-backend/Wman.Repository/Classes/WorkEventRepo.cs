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
        public void Add(WorkEvent element)
        {
            this.db.Add(element);
            this.db.SaveChanges();
        }

        public void Delete(int element)
        {
            this.db.Remove(element);
            this.db.SaveChanges();
        }

        public IQueryable<WorkEvent> GetAll()
        {
            return this.db.WorkEvent;
        }

        public WorkEvent GetOne(int key)
        {
            var entity = (from x in db.WorkEvent
                          where x.Id == key
                          select x).FirstOrDefault();
            return entity;
        }

        public void Update(int oldKey, WorkEvent element)
        {
            var oldWorkEvent = GetOne(oldKey);
            oldWorkEvent.JobDescription = element.JobDescription;
            oldWorkEvent.EstimatedStartDate = element.EstimatedStartDate;
            oldWorkEvent.EstimatedFinishDate = element.EstimatedFinishDate;
            this.db.SaveChanges();
        }
    }
}
