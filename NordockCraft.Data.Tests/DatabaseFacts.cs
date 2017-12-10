using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Tests
{
    public abstract class DatabaseFacts
    {
        private DbContextOptions<CraftingContext> _options;

        protected DatabaseFacts()
        {
            using (var context = Context)
            {
                context.Database.EnsureCreated();
            }
        }

        protected CraftingContext Context => new CraftingContext(Options);

        private ILoggerFactory LoggerFactory => new LoggerFactory(LoggerProviders);
        public IEnumerable<ILoggerProvider> LoggerProviders => new[] {new ConsoleLoggerProvider((_, __) => true, true)};

        private DbContextOptions<CraftingContext> Options
        {
            get
            {
                if (_options == null)
                {
                    var builder = new DbContextOptionsBuilder<CraftingContext>();
                    ModifyOptions(builder);
                    builder.EnableSensitiveDataLogging();
                    builder.UseLoggerFactory(LoggerFactory);
                    _options = builder.Options;
                }
                return _options;
            }
        }

        protected abstract void ModifyOptions(DbContextOptionsBuilder<CraftingContext> builder);
    }
}