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
    public class InventoryService : IInventoryService
    {
        private readonly AppDbContext _context;

        public InventoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetInventoryAsync(int characterId)
        {
            
            var character = await _context.Characters
                                          .Include(c => c.Inventory)
                                          .FirstOrDefaultAsync(c => c.Id == characterId); 

            return character?.Inventory?.ToList() ?? [];
        }

        public async Task<bool> AddItemToInventoryAsync(int characterId, int itemId)
        {
            var characterTask = _context.Characters
                                        .Include(c => c.Inventory)
                                        .FirstOrDefaultAsync(c => c.Id == characterId); 
            var itemToAddTask = _context.Items.FindAsync(itemId).AsTask();
            await Task.WhenAll(characterTask, itemToAddTask);

            var character = await characterTask;
            var itemToAdd = await itemToAddTask;

            if (character == null || itemToAdd == null)
            {
                return false;
            }

            character.Inventory ??= [];
            character.Inventory.Add(itemToAdd);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveItemFromInventoryAsync(int characterId, int itemId)
        {
            
            var character = await _context.Characters
                                          .Include(c => c.Inventory)
                                          .FirstOrDefaultAsync(c => c.Id == characterId); 

            if (character == null || character.Inventory == null)
            {
                return false;
            }
            var itemToRemove = character.Inventory.FirstOrDefault(i => i.Id == itemId);

            if (itemToRemove == null)
            {
                return false;
            }

            character.Inventory.Remove(itemToRemove);

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
