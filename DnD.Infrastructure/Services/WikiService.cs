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
    public class WikiService : IWikiService
    {
        private readonly AppDbContext _context;

        public WikiService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<CharacterClass>> GetAllClassesAsync()
        {
            return await _context.CharacterClasses.ToListAsync();
        }
        public async Task<CharacterClass?> GetClassDetailsAsync(int classId)
        {
            return await _context.CharacterClasses.FindAsync(classId);
        }
        public async Task<List<CharacterRace>> GetAllRacesAsync()
        {
            return await _context.CharacterRaces.ToListAsync();
        }
        public async Task<CharacterRace?> GetRaceDetailsAsync(int raceId)
        {
            return await _context.CharacterRaces
                                 .Include(r => r.Traits)
                                 .FirstOrDefaultAsync(r => r.Id == raceId);
        }
        public async Task<List<Spell>> GetAllSpellsAsync()
        {
            return await _context.Spell
                                 .OrderBy(s => s.Level)
                                 .ThenBy(s => s.Name)
                                 .ToListAsync();
        }

        public async Task<Spell?> GetSpellDetailsAsync(int spellId)
        {
            return await _context.Spell.FindAsync(spellId);
        }

    }
}
