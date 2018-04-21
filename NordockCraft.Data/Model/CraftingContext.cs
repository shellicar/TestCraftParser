using Microsoft.EntityFrameworkCore;

namespace NordockCraft.Data.Model
{
    public class CraftingContext : DbContext
    {
        public CraftingContext(DbContextOptions<CraftingContext> options) : base(options)
        {
        }

        public CraftingContext()
        {
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipeLocation> RecipeLocations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=crafting.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredient>(e =>
            {
                e.HasKey(x => new {x.RecipeId, x.IngredientId});

                e.HasOne(x => x.Recipe)
                    .WithMany()
                    .HasForeignKey(x => x.RecipeId)
                    .IsRequired();

                e.HasOne(x => x.Ingredient)
                    .WithMany(x => x.RecipeIngredients)
                    .HasForeignKey(x => x.IngredientId)
                    .IsRequired();
            });

            modelBuilder.Entity<Item>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasAlternateKey(x => x.Name);

                e.Property(x => x.Name)
                    .IsRequired();

                e.HasOne(x => x.Recipe)
                    .WithOne(x => x.Item)
                    .IsRequired();

                e.HasMany(x => x.IngredientFor)
                    .WithOne(x => x.Item);
            });

            modelBuilder.Entity<Location>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasAlternateKey(x => x.Name);

                e.Property(x => x.Name)
                    .IsRequired();

                e.HasMany(x => x.RecipeLocations)
                    .WithOne(x => x.Location);
            });

            modelBuilder.Entity<Recipe>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasAlternateKey(x => x.ItemId);

                e.HasOne(x => x.Item)
                    .WithOne(x => x.Recipe)
                    .HasForeignKey<Recipe>(x => x.ItemId)
                    .IsRequired();

                e.HasMany(x => x.RecipeLocations)
                    .WithOne(x => x.Recipe);

                e.HasMany(x => x.RecipeIngredients)
                    .WithOne(x => x.Recipe);
            });

            modelBuilder.Entity<Ingredient>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasAlternateKey(x => new {x.ItemId, x.Amount});

                e.HasOne(x => x.Item)
                    .WithMany(x => x.IngredientFor)
                    .HasForeignKey(x => x.ItemId)
                    .IsRequired();

                e.HasMany(x => x.RecipeIngredients)
                    .WithOne(x => x.Ingredient);
            });

            modelBuilder.Entity<RecipeLocation>(e =>
            {
                e.HasKey(x => new {x.RecipeId, x.LocationId});

                e.HasOne(x => x.Recipe)
                    .WithMany(x => x.RecipeLocations)
                    .HasForeignKey(x => x.RecipeId)
                    .IsRequired();

                e.HasOne(x => x.Location)
                    .WithMany(x => x.RecipeLocations)
                    .HasForeignKey(x => x.LocationId)
                    .IsRequired();
            });
        }
    }
}