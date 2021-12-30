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
    public class ProofOfWorkRepo : IProofOfWorkRepo
    {
        private wmanDb db;
        public ProofOfWorkRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public async Task Add(ProofOfWork element)
        {
            await this.db.AddAsync(element);
            await this.db.SaveChangesAsync();

        }

        public async Task Delete(int element)
        {
            var entity = await GetOne(element);
            this.db.Remove(entity);
            await this.db.SaveChangesAsync();
        }

        public IQueryable<ProofOfWork> GetAll()
        {
            return this.db.ProofOfWork;
        }

        public async Task<ProofOfWork> GetOne(int key)
        {
            var entity = await(from x in db.ProofOfWork
                               where x.Id == key
                               select x).FirstOrDefaultAsync();
            return entity;
        }

        public async Task SaveDatabase()
        {
            await this.db.SaveChangesAsync();
        }

        public async Task Update(int oldKey, ProofOfWork element)
        {
            var oldPicture = await GetOne(oldKey);
            oldPicture.Name = element.Name;
            oldPicture.Url = element.Url;
            await db.SaveChangesAsync();
        }
    }
}
