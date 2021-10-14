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
    public class WmanUserRepo : IWmanUserRepo
    {
        private wmanDb db;
        public WmanUserRepo(wmanDb inDb)
        {
            this.db = inDb;
        }

        public async Task<WmanUser> getUser(string username)
        {
            var entity = await db.Users
                .Where(x => x.UserName == username)
                .Include(x => x.WorkEvents)
                .SingleOrDefaultAsync();
            return entity;
        }
    }
}
