using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.DTOs
{
    public class CharacterCreationDto
    {

        public string Name { get; set; }
        public int PlayerId { get; set; }
        public int RaceId { get; set; }
        public int ClassId { get; set; }
        public Dictionary<string, int> BaseStats { get; set; }
        public List<int> SelectedSpellIds { get; set; }

    }
}
