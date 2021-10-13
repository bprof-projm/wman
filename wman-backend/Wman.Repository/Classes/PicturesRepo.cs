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
    public class PicturesRepo : IPicturesRepo
    {
        private wmanDb db;
        public PicturesRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public async Task Add(Pictures element)
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

        public IQueryable<Pictures> GetAll()
        {
            return this.db.Picture;
        }

        public async Task<Pictures> GetOne(int key)
        {
            var entity =await (from x in db.Picture
                          where x.Id == key
                          select x).FirstOrDefaultAsync();
            return entity;
        }

        public async Task Update(int oldKey, Pictures element)
        {
            var oldPicture =await GetOne(oldKey);
            oldPicture.Name = element.Name;
            oldPicture.Url = element.Url;
            oldPicture.PicturesType = element.PicturesType;
            await db.SaveChangesAsync();
        }
    }
}
