using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IUserService
    {
        Task<User?> RegisterAsync(string email, string password);
        Task<User?> AuthenticateAsync(string email, string password);
    }
}
