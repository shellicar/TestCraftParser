namespace NordockCraft.Data.Exceptions
{
    public class RecipeLocationAlreadyExistsError : UserError
    {
        public override string Message => "Recipe location already exists";
    }
}