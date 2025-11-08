using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Model;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}
