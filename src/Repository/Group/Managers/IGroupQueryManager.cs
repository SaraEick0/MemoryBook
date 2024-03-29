﻿namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IGroupQueryManager
    {
        Task<IList<GroupReadModel>> GetAllGroups(Guid memoryBookUniverseId);

        Task<IList<GroupReadModel>> GetGroups(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}