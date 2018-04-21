using System.Collections.Generic;

namespace NordockCraft.Data.Model
{
    public class Location
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual List<RecipeLocation> RecipeLocations { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}";
        }
    }
}