using DnD.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnD.Domain.Services
{
    public interface IInventoryService
    {
        Task<List<Item>> GetInventoryAsync(int characterId);
        Task<bool> AddItemToInventoryAsync(int characterId, int itemId);
        Task<bool> RemoveItemFromInventoryAsync(int characterId, int itemId);
    }
}
