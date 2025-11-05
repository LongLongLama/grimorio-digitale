using DnD.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ItemType Type { get; set; }
        public ICollection<ItemStat>? Stats { get; set; }

        [InverseProperty(nameof(Character.Inventory))]
        public ICollection<Character>? Characters { get; set; }

        [InverseProperty(nameof(Character.EquippedArmor))]
        public ICollection<Character>? ArmorWearers { get; set; }

        [InverseProperty(nameof(Character.PrimaryWeapon))]
        public ICollection<Character>? PrimaryWielders { get; set; }

        [InverseProperty(nameof(Character.SecondaryWeapon))]
        public ICollection<Character>? SecondaryWielders { get; set; }
        public int? ArmorClass { get; set; }
        public string? Damage { get; set; }
        public string? DamageType { get; set; }


    }
}
