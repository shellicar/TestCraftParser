using System;

namespace NordockCraft.Data.Exceptions
{
    public abstract class UserError : Exception
    {
        public abstract override string Message { get; }
    }
}