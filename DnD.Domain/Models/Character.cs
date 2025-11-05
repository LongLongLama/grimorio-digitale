using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Entities
{
    public class Character
    {

        public int Id { get; set; }
        public required string Name { get; set; }
        public short Level { get; set; }
        public short MaxHitPoints { get; set; }
        public short CurrentHitPoints { get; set; }
        public required Player Player { get; set; }
        public required CharacterClass Class { get; set; }
        public required CharacterRace Race { get; set; }

        [InverseProperty(nameof(Item.Characters))]
        public ICollection<Item>? Inventory { get; set; }
        public ICollection <Spell>? KnownSpells { get; set; }

        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public int? EquippedArmorId { get; set; }
        
        [ForeignKey(nameof(EquippedArmorId))]
        public Item? EquippedArmor { get; set; }
        public int? PrimaryWeaponId { get; set; }

        [ForeignKey(nameof(PrimaryWeaponId))]
        public Item? PrimaryWeapon { get; set; }
        public int? SecondaryWeaponId { get; set; }

        [ForeignKey(nameof(SecondaryWeaponId))]
        public Item? SecondaryWeapon { get; set; }





    }
}
