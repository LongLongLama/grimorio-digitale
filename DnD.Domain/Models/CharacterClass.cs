using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class CharacterClass
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Descritpion { get; set; }
        public short HitDice { get; set; }
        public ICollection<Spell>? ClassSpells { get; set; }
        public ICollection<Character>? Characters { get; set; }


    }
}
