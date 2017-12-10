using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Tests
{
    public abstract class InMemoryDatabaseFacts : DatabaseFacts
    {
        protected override void ModifyOptions(DbContextOptionsBuilder<CraftingContext> builder)
        {
            builder.UseInMemoryDatabase("test.db");
        }
    }
}