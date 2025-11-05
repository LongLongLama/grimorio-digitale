using DnD.Domain.DTOs;
using DnD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Services
{
    public interface ICharacterCalculatorService
    {

        Character CalculateNewCharacter(CharacterCreationDto data, Player player, CharacterRace race, CharacterClass charClass);

        int CalculateLevelUpHitPoints(Character character);

        int CalculateArmorClass(Character character);

        string CalculateWeaponDamage(Character character);

    }
}
