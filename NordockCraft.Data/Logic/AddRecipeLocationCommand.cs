using System.Linq;
using NordockCraft.Data.Access;
using NordockCraft.Data.Exceptions;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Logic
{
    internal class AddRecipeLocationCommand : IAddRecipeLocationCommand
    {
        public AddRecipeLocationCommand(ICraftingAccess access)
        {
            Access = access;
        }

        private ICraftingAccess Access { get; }

        public void AddLocation(Recipe recipe, Location location)
        {
            var existing = Access.RecipeLocations.SingleOrDefault(x => x.Recipe == recipe && x.Location == location);
            if (existing != null)
            {
                throw new RecipeLocationAlreadyExistsError();
            }

            var recipeLocation = new RecipeLocation {Recipe = recipe, Location = location};
            Access.AddRecipeLocation(recipeLocation);
        }
    }
}