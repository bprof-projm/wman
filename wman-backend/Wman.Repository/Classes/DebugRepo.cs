using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data;
using Wman.Data.DB_Models;

namespace Wman.Repository
{
    public class debugRepo : IDebug
    {
        private wmanDb db;
        public debugRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public void Add(WorkEvent element)
        {
            db.Add(element);
            db.SaveChanges();
        }

        public void Delete(int element)
        {
            throw new NotImplementedException();
        }

        public IQueryable<WorkEvent> GetAll()
        {
            return this.db.WorkEvent;
        }

        public WorkEvent GetOne(int key)
        {
            throw new NotImplementedException();
        }

        public void Update(int oldKey, WorkEvent element)
        {
            throw new NotImplementedException();
        }
    }

}
