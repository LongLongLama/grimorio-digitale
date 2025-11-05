using DnD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class ItemStat
    {
        public int Id { get; set; }
        public StatType StatType { get; set; }
        public int Value { get; set; }
        public required Item Item { get; set; }

    }
}
