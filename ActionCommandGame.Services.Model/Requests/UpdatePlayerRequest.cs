using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Services.Model.Requests
{
    public class UpdatePlayerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
