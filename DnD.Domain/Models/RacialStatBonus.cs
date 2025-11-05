using DnD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class RacialStatBonus
    {
        public int Id { get; set; }
        public StatType Type { get; set; }
        public short BonusValue { get; set; }
        public required CharacterRace Race { get; set; }


       
    }
}
