namespace MemoryBook.Repository.MemoryBookUniverse.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IMemoryBookUniverseCommandManager
    {
        Task<IList<Guid>> CreateMemoryBookUniverse(params MemoryBookUniverseCreateModel[] models);

        Task DeleteMemoryBookUniverse(params Guid[] groupIds);
    }
}