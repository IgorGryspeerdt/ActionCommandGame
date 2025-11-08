using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace ActionCommandGame.Services
{
    public class UserService : IUserService
    {
        private readonly ActionCommandGameDbContext _dbContext;

        public UserService(ActionCommandGameDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> RegisterAsync(string email, string password)
        {
            if (await _dbContext.Users.AnyAsync(u => u.Email == email))
            {
                return null; 
            }

            var user = new User
            {
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)) return null;
            return user;
        }
    }
}
