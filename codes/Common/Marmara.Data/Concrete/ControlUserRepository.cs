using Marmara.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marmara.Data.Concrete
{
    public class ControlUserRepository
    {

        public async Task<int> CreateNewUserAsync(ControlUser user)
        {
            using var dbContext = new MarmaraDbContext();
            dbContext.Users.Add(user);
            return await dbContext.SaveChangesAsync();
        }


        public async Task<int> DeleteUserAsync(int id)
        {
            using var dbContext = new MarmaraDbContext();
            var entity = dbContext.Users.Find(id);
            entity.IsActive = false;
            dbContext.Users.Update(entity);
            return await dbContext.SaveChangesAsync();
        }  
        
        public async Task<ControlUser> GetUserAsync(int id)
        {
            using var dbContext = new MarmaraDbContext();
            var entity = dbContext.Users.FindAsync(id);
            return await entity;
        }

        public async Task<List<ControlUser>> GetUserListAsync()
        {
            using var dbContext = new MarmaraDbContext();
            return await dbContext.Users.Where(x => x.IsActive == true).ToListAsync();
        } 

    }
}
