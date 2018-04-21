using NordockCraft.Data.Model;

namespace NordockCraft.Data.Logic
{
    public interface ICreateRecipeCommand
    {
        void CreateRecipe(Recipe recipe);
    }
}