namespace MemoryBook.DataAccess.Interfaces
{
    using System;

    public interface IHasIdProperty
    {
        Guid Id { get; set; }
    }
}