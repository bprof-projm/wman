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
    public class LabelRepo : ILabelRepo
    {
        private wmanDb db;
        public LabelRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public async Task Add(Label element)
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

        public IQueryable<Label> GetAll()
        {
            return this.db.Label;
        }

        public async Task<Label> GetOne(int key)
        {
            var entity = await (from x in db.Label
                          where x.Id == key
                          select x).Include(x => x.WorkEvents).FirstOrDefaultAsync();
            return entity;
        }

        public async Task Update(int oldKey, Label element)
        {
            var oldLabel =await GetOne(oldKey);
            oldLabel.Color = element.Color;
            oldLabel.Content = element.Content;
            await this.db.SaveChangesAsync();
        }
        public async Task SaveDatabase()
        {
            await this.db.SaveChangesAsync();
        }
    }
}
