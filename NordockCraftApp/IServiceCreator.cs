using NordockCraft.Data.Service;

namespace WindowsFormsApp3
{
    public interface IServiceCreator
    {
        ILookupItemService LookupService { get; }
    }
}