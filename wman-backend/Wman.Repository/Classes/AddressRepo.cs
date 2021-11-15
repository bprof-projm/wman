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
    public class AddressRepo : IAddressRepo
    {
        private wmanDb db;
        public AddressRepo(wmanDb inDb)
        {
            this.db = inDb;
        }
        public async Task Add(AddressHUN element)
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

        public IQueryable<AddressHUN> GetAll()
        {
            return this.db.Address;
        }

        public async Task<AddressHUN> GetOne(int key)
        {
            var entity =await (from x in db.Address
                          where x.Id == key
                          select x).FirstOrDefaultAsync();
            return entity;
        }

        public async Task Update(int oldKey, AddressHUN element)
        {
            var oldAddress =await GetOne(oldKey);
            oldAddress.City = element.City;
            oldAddress.Street = element.Street;
            oldAddress.ZIPCode = element.ZIPCode;
            await this.db.SaveChangesAsync();
        }
    }
}
