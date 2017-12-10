using System;
using System.Collections.Immutable;

namespace NordockCraft.Data.Service
{
    public interface ILookupItemService : IDisposable
    {
        ItemDto LookupItem(string name);
        IImmutableList<ItemDto> LookupSimilar(string text);
    }
}