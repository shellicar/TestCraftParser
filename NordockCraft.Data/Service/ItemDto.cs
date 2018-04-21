using System.Collections.Immutable;

namespace NordockCraft.Data.Service
{
    public class ItemDto
    {
        public string Name { get; set; }
        public RecipeDto CreatedBy { get; set; }
        public IImmutableList<RecipeDto> UsedIn { get; set; }
        public IImmutableList<LocationDto> CreatedAtLocations { get; set; }
        public IImmutableList<LocationDto> UsedAtLocations { get; set; }
    }
}