using NordockCraft.Data.Model;

namespace NordockCraft.Data.Logic
{
    public interface IAddRecipeLocationCommand
    {
        void AddLocation(Recipe recipe, Location location);
    }
}