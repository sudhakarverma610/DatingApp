using DatingApp.Api.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Api.Data
{
    public class DatingRespository : IDatingRespository
    {
        private DataContext _context;

        public DatingRespository(DataContext context)
        {
            _context = context;
        }
        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public  async Task<Photo> GetMainPhoto(int userId)
        {
            var photo = await _context.Photos.Where(p => p.UserId == userId).FirstOrDefaultAsync(x=>x.IsMain);
            return photo;
        }

        public async Task<Photo> GetPhoto(int Id)
        {
            var photo =await _context.Photos.FirstOrDefaultAsync(p => p.Id == Id);
            return photo;
         }

        public async Task<User> GetUser(int Id)
        {
            var  user= await  _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(user => user.Id == Id);
            return user;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
