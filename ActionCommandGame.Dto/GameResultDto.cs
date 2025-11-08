using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Dto
{
    public class GameResultDto
    {
        public PlayerDto Player { get; set; }
        public PositiveGameEventDto? PositiveGameEvent { get; set; }
        public NegativeGameEventDto? NegativeGameEvent { get; set; }
        public List<string>? NegativeGameEventMessages { get; set; }
    }
}
