﻿using System;
using NordockCraft.Data.Service;

namespace TestCraftParser
{
    public interface IServiceProvider : IDisposable
    {
        ICreateRecipeService Service { get; }
        void CreateDatabase();
    }
}