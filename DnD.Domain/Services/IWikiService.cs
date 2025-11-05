using DnD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Services
{
    public interface IWikiService
    {
        Task<List<CharacterRace>> GetAllRacesAsync();
        Task<CharacterRace?> GetRaceDetailsAsync(int raceId);
        Task<List<CharacterClass>> GetAllClassesAsync();
        Task<CharacterClass?> GetClassDetailsAsync(int classId);
        Task<List<Spell>> GetAllSpellsAsync();
        Task<Spell?> GetSpellDetailsAsync(int spellId);
    }
}
