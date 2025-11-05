using DnD.Domain.DTOs;
using DnD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Services
{
    public interface ICharacterService
    {
        Task<List<Character>> GetAllCharactersAsync();
        Task<Character?> GetCharacterByIdAsync(int characterId);
        Task<List<Character>> GetCharactersByPlayerIdAsync(int playerId);
        Task<Character?> CreateCharacterAsync(CharacterCreationDto data);
        Task<Character?> UpdateCharacterNameAsync(int characterId, string newName);
        Task<bool> EquipItemAsync(int characterId, int? itemId, string slot);
        Task<bool> UnequipItemAsync(int characterId, string slot);
        Task<bool> DeleteCharacterAsync(int characterId);
    }
}
