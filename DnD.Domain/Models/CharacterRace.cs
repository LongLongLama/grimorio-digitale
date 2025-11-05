using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class CharacterRace
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public short BaseSpeed { get; set; }
        public ICollection<RacialStatBonus>? StatBonuses { get; set; }
        public ICollection<RacialTrait>? Traits { get; set; }
        public ICollection<Character>? Characters { get; set; }


    }
}
