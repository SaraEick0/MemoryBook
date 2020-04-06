namespace MemoryBook.Business.Detail.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using MemoryBook.Business.Detail.Models;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;

    public interface IDetailProvider
    {
        Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, MemberReadModel creator, string customDetailText,
            DetailTypeEnum detailType, DateTime? startDate, DateTime? endDate = null);

        Task<IList<DetailsByEntityModel>> GetDetailsForMembers(Guid memoryBookUniverseId, params Guid[] memberIds);

        Task<IList<DetailsByEntityModel>> GetDetailsForRelationships(Guid memoryBookUniverseId, params Guid[] relationshipIds);

        Task<IList<DetailsByEntityModel>> GetDetailsForGroups(Guid memoryBookUniverseId, params Guid[] groupIds);
    }
}