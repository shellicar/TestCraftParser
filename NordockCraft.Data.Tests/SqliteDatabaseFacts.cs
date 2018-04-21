using System;
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Tests
{
    public abstract class SqliteDatabaseFacts : DatabaseFacts, IDisposable
    {
        private DbConnection Connection { get; } = new SqliteConnection("Data Source=:memory:");

        public void Dispose()
        {
            Connection?.Dispose();
        }

        protected override void ModifyOptions(DbContextOptionsBuilder<CraftingContext> builder)
        {
            builder.UseSqlite(Connection);
            Connection.Open();
        }
    }
}