using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Access;
using NordockCraft.Data.Model;
using NordockCraft.Data.Service;

namespace TestCraftParser
{
    public class ServiceProvider : IServiceProvider
    {
        private DbConnection _connection;
        private ICraftingAccess Access => new CraftingAccess(Context);

        private DbContextOptions<CraftingContext> ContextOptions
        {
            get
            {
                var builder = new DbContextOptionsBuilder<CraftingContext>();
                builder.UseSqlite(Connection);
                return builder.Options;
            }
        }

        private DbConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqliteConnection("Data Source=crafting.db");
                    _connection.Open();
                }
                return _connection;
            }
        }

        private CraftingContext Context => new CraftingContext(ContextOptions);

        public ICreateRecipeService Service => new CreateRecipeService(Access);

        public void CreateDatabase()
        {
            using (var context = Context)
            {
                context.Database.EnsureCreated();
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}