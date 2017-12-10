using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NordockCraft.Data.Access;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Service
{
    public class LookupItemService : ILookupItemService
    {
        public LookupItemService(ICraftingAccess access)
        {
            Access = access;
        }

        private ICraftingAccess Access { get; }

        public void Dispose()
        {
            Access?.Dispose();
        }

        public ItemDto LookupItem(string name)
        {
            var item = Access.Items.SingleOrDefault(x => x.Name == name);
            if (item == null)
            {
                return null;
            }

            var usedIn = from ingredient in item.IngredientFor
                from recipeIngredient in ingredient.RecipeIngredients
                orderby recipeIngredient.Recipe.Item.Name
                select CreateRecipeDto(recipeIngredient.Recipe);

            /*var usedAt = from ingredient in item.IngredientFor
                where ingredient.RecipeIngredients != null
                from recipeIngredient in ingredient.RecipeIngredients
                where recipeIngredient.Recipe != null
                from locabc in recipeIngredient.Recipe.RecipeLocations
                select CreateLocationDto(locabc.Location);*/

            var usedAt = (from ingredient in item.IngredientFor
                from ri in ingredient.RecipeIngredients
                select ri.Recipe).Distinct();

            var usedAtLocations = (from rec in usedAt
                from test in rec.RecipeLocations
                select test.Location).Distinct().Select(CreateLocationDto);


            IEnumerable<LocationDto> locations;

            if (item.Recipe == null)
            {
                locations = Enumerable.Empty<LocationDto>();
            }
            else
            {
                locations = from locs in item.Recipe.RecipeLocations
                    orderby locs.Location.Name
                    select CreateLocationDto(locs.Location);
            }


            var dto = new ItemDto
            {
                CreatedBy = CreateRecipeDto(item.Recipe),
                Name = item.Name,
                CreatedAtLocations = locations.ToImmutableList(),
                UsedAtLocations = usedAtLocations.ToImmutableList(),
                UsedIn = usedIn.ToImmutableList()
            };
            return dto;
        }

        public IImmutableList<ItemDto> LookupSimilar(string text)
        {
            var query = from item in Access.Items
                where EF.Functions.Like(item.Name, $"%{text}%")
                orderby item.Name
                select item;

            return query.Select(ConvertToDto).ToImmutableList();
        }

        private static LocationDto CreateLocationDto(Location locs)
        {
            if (locs == null)
            {
                throw new InvalidOperationException();
            }

            return new LocationDto {Id = locs.Id, Name = locs.Name};
        }

        private ItemDto ConvertToDto(Item arg)
        {
            return new ItemDto {Name = arg.Name};
        }

        private List<RecipeDto> CreateUsed(Item item)
        {
            var recipes = item.IngredientFor.SelectMany(x => x.RecipeIngredients).Select(x => x.Recipe);
            return recipes.Select(CreateRecipeDto).ToList();
        }

        private RecipeDto CreateRecipeDto(Recipe arg)
        {
            if (arg == null)
            {
                return null;
            }

            var ingredients = (from q in arg.RecipeIngredients
                orderby q.Ingredient.Item.Name
                select ConvertIngredient(q)).ToImmutableList();

            return new RecipeDto
            {
                Id = arg.Id,
                ItemCreated = arg.Item.Name,
                Ingredients = ingredients
            };
        }

        private IngredientDto ConvertIngredient(RecipeIngredient arg)
        {
            if (arg == null)
            {
                return null;
            }

            var dto = new IngredientDto
            {
                Amount = arg.Ingredient.Amount,
                ItemName = arg.Ingredient.Item.Name
            };
            return dto;
        }
    }
}