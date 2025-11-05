using DnD.Domain.DTOs;
using DnD.Domain.Entities;
using DnD.Domain.Enums;
using DnD.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Infrastructure.Services
{
    public class CharacterCalculatorService : ICharacterCalculatorService
    {
        private static readonly Random _random = new();
        public CharacterCalculatorService() { }

        public Character CalculateNewCharacter(CharacterCreationDto data, Player player, CharacterRace race, CharacterClass charClass)
        {
            var statsFinali = new Dictionary<string, int>();

            statsFinali["Forza"] = data.BaseStats["Forza"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Strength)?.BonusValue ?? 0);
            statsFinali["Destrezza"] = data.BaseStats["Destrezza"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Dexterity)?.BonusValue ?? 0);
            statsFinali["Costituzione"] = data.BaseStats["Costituzione"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Constitution)?.BonusValue ?? 0);
            statsFinali["Intelligenza"] = data.BaseStats["Intelligenza"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Intelligence)?.BonusValue ?? 0);
            statsFinali["Saggezza"] = data.BaseStats["Saggezza"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Wisdom)?.BonusValue ?? 0);
            statsFinali["Carisma"] = data.BaseStats["Carisma"] + (race.StatBonuses?.FirstOrDefault(b => b.Type == StatType.Charisma)?.BonusValue ?? 0);

            int modificatoreCostituzione = (int)((statsFinali["Costituzione"] - 10) / 2.0);

            int maxHitPoints = charClass.HitDice + modificatoreCostituzione;

            var nuovoPersonaggio = new Character
            {
                Player = player,
                Name = data.Name,
                Level = 1,
                MaxHitPoints = (short)maxHitPoints,
                CurrentHitPoints = (short)maxHitPoints,
                Race = race,
                Class = charClass,
                Strength = statsFinali["Forza"],
                Dexterity = statsFinali["Destrezza"],
                Constitution = statsFinali["Costituzione"],
                Intelligence = statsFinali["Intelligenza"],
                Wisdom = statsFinali["Saggezza"],
                Charisma = statsFinali["Carisma"],
                KnownSpells = [],
                Inventory = []
            };

            return nuovoPersonaggio;
        }

        public int CalculateLevelUpHitPoints(Character character)
        {
           
            int hitDieRoll = _random.Next(1, character.Class.HitDice + 1);


            int constitutionModifier = (int)((character.Constitution - 10) / 2.0);

            return Math.Max(1, hitDieRoll + constitutionModifier);
        }


        public int CalculateArmorClass(Character character)
        {
            int dexterityModifier = (int)((character.Dexterity - 10) / 2.0);
            int classeArmatura = 10 + dexterityModifier; 

            if (character.EquippedArmor != null && character.EquippedArmor.Type == ItemType.Armor)
            {
                int armorBaseAC = character.EquippedArmor.ArmorClass ?? 0;

                if (armorBaseAC >= 16)
                {
                    classeArmatura = armorBaseAC;
                }
                else
                {
                    classeArmatura = armorBaseAC + dexterityModifier;
                }
            }

            if (character.SecondaryWeapon != null && character.SecondaryWeapon.Type == ItemType.Armor)
            {
                classeArmatura += character.SecondaryWeapon.ArmorClass ?? 0;
            }

            return classeArmatura;
        }

        public string CalculateWeaponDamage(Character character)
        {
            int strengthModifier = (int)((character.Strength - 10) / 2.0);

            if (character.PrimaryWeapon == null || string.IsNullOrEmpty(character.PrimaryWeapon.Damage))
            {

                return $"1 + {strengthModifier} Contundente (Pugno)";
            }

            string modifierString = strengthModifier >= 0 ? $"+{strengthModifier}" : strengthModifier.ToString();
            return $"{character.PrimaryWeapon.Damage} {modifierString} {character.PrimaryWeapon.DamageType}";
        }
    
}
}
