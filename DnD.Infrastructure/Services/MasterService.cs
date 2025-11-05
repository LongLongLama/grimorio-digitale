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
    public class MasterService : IMasterService
    {
        private readonly AppDbContext _context;
        private readonly ICharacterCalculatorService _calculator;
        public MasterService(AppDbContext context, ICharacterCalculatorService calculator)
        {
            _context = context;
            _calculator = calculator;
        }

        public async Task<List<Character>> LevelUpAllCharactersAsync()
        {
            var allCharacters = await _context.Characters.Include(c => c.Class).ToListAsync();

            if (!allCharacters.Any())
            {
                return new List<Character>();
            }

            foreach (var character in allCharacters)
            {
                int hpIncrease = _calculator.CalculateLevelUpHitPoints(character);

                character.Level++;
                character.MaxHitPoints += (short)hpIncrease;
                character.CurrentHitPoints = character.MaxHitPoints;
            }

            await _context.SaveChangesAsync();

            return allCharacters;
        }
    }
}
