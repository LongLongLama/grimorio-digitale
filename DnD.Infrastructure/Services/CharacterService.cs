using DnD.Domain.DTOs;
using DnD.Domain.Entities;
using DnD.Domain.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Infrastructure.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly AppDbContext _context;
        private readonly ICharacterCalculatorService _calculator;

        public CharacterService(AppDbContext context, ICharacterCalculatorService calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<Character?> CreateCharacterAsync(CharacterCreationDto data)
        {

            var playerTask = _context.Players.FindAsync(data.PlayerId).AsTask();
            var raceTask = _context.CharacterRaces.Include(r => r.StatBonuses).FirstOrDefaultAsync(r => r.Id == data.RaceId);
            var charClassTask = _context.CharacterClasses.FindAsync(data.ClassId).AsTask();
            var spellsTask = _context.Spell.Where(s => data.SelectedSpellIds.Contains(s.Id)).ToListAsync();

            await Task.WhenAll(playerTask, raceTask, charClassTask, spellsTask);

            var player = await playerTask;
            var race = await raceTask;
            var charClass = await charClassTask;
            var spells = await spellsTask;

            if (player == null || race == null || charClass == null)
            {
                return null;
            }

            var nuovoPersonaggio = _calculator.CalculateNewCharacter(data, player, race, charClass);

            foreach (var spell in spells)
            {
                nuovoPersonaggio.KnownSpells.Add(spell);
            }

            _context.Characters.Add(nuovoPersonaggio);
            await _context.SaveChangesAsync();

            return nuovoPersonaggio;
        }

        public async Task<List<Character>> GetAllCharactersAsync()
        {
            return await _context.Characters
                .Include(c => c.Race)
                .ToListAsync();
        }

        public async Task<Character?> GetCharacterByIdAsync(int characterId)
        {
            return await _context.Characters
                .Include(c => c.Race)
                .Include(c => c.Class)
                .Include(c => c.Player)
                .Include(c => c.EquippedArmor)
                .Include(c => c.PrimaryWeapon)
                .Include(c => c.SecondaryWeapon)
                .Include(c => c.KnownSpells)
                .FirstOrDefaultAsync(c => c.Id == characterId); 
        }

        public async Task<List<Character>> GetCharactersByPlayerIdAsync(int playerId)
        {
            return await _context.Characters
                .Where(c => c.Player.Id == playerId) 
                .Include(c => c.Race)
                .Include(c => c.Class)
                .ToListAsync(); 
        }

        public async Task<Character?> UpdateCharacterNameAsync(int characterId, string newName)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null) return null;

            character.Name = newName;
            await _context.SaveChangesAsync();
            return character;
        }

        public async Task<bool> EquipItemAsync(int characterId, int? itemId, string slot)
        {
            var character = await _context.Characters.FindAsync(characterId); 
            if (character == null) return false;

            switch (slot.ToLower())
            {
                case "armor": character.EquippedArmorId = itemId; break;
                case "primary": character.PrimaryWeaponId = itemId; break;
                case "secondary": character.SecondaryWeaponId = itemId; break;
                default: return false;
            }

            await _context.SaveChangesAsync(); 
            return true;
        }

        public async Task<bool> UnequipItemAsync(int characterId, string slot)
        {
            return await EquipItemAsync(characterId, null, slot);
        }

        public async Task<bool> DeleteCharacterAsync(int characterId)
        {
            var character = await _context.Characters.FindAsync(characterId);
            if (character == null) return false;

            _context.Characters.Remove(character);
            await _context.SaveChangesAsync(); 
            return true;
        }

    }
}
