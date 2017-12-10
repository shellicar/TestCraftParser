using System;
using NordockCraft.Data.Model;
using NordockCraft.Data.Service;

namespace TestCraftParser
{
    public interface IServiceProvider : IDisposable
    {
        ICreateRecipeService Service { get; }
        CraftingContext Context { get; }
    }
}