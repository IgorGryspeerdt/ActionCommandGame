using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionCommandGame.Dto
{
    public class NegativeGameEventDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? DefenseWithGearDescription { get; set; }
        public string? DefenseWithoutGearDescription { get; set; }
        public int DefenseLoss { get; set; }
    }
}
