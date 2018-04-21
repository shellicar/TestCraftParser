namespace NordockCraft.Data.Model
{
    public class RecipeLocation
    {
        public virtual Recipe Recipe { get; set; }
        public virtual int RecipeId { get; set; }
        public virtual Location Location { get; set; }
        public virtual int LocationId { get; set; }
    }
}