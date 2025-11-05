using DnD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class Spell
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public short Level { get; set; }
        public string? CastingTime { get; set; }
        public short Range { get; set; }
        public short Duration { get; set; }
        public MagicSchool School { get; set; }
        public ICollection<Character>? Characters { get; set; }
        public required ICollection<CharacterClass> Classes { get; set; }


    }
}
