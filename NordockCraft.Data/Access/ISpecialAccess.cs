using System;
using System.Linq;
using NordockCraft.Data.Model;

namespace NordockCraft.Data.Access
{
    public interface ISpecialAccess : IDisposable
    {
        IQueryable<Item> Items { get; }
        IQueryable<Ingredient> Ingredients { get; }
    }
}