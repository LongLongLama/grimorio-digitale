using DnD.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace DnD.Infrastructure
{

    public class AppDbContext : DbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Character> Characters { get; init; }
        public DbSet<CharacterClass> CharacterClasses { get; init; }
        public DbSet<CharacterRace> CharacterRaces { get; init; }
        public DbSet<Item> Items { get; init; }
        public DbSet<ItemStat> ItemStats { get; init; }
        public DbSet<Player> Players { get; init; }
        public DbSet<RacialStatBonus> RacialStatBonus { get; init; }
        public DbSet<RacialTrait> RacialTraits { get; init; }
        public DbSet<Spell> Spell {  get; init; }

        

      


    }
}
