using NordockCraft.Data.Access;
using NordockCraft.Data.Model;
using NordockCraft.Data.Service;

namespace WindowsFormsApp3
{
    public class ServiceCreator : IServiceCreator
    {
        public ICraftingAccess Access => new CraftingAccess(Context);
        private CraftingContext Context => new CraftingContext();
        public ILookupItemService LookupService => new LookupItemService(Access);
    }
}