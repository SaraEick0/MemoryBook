namespace MemoryBook.Business.Detail.Providers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.Member.Models;

    public interface IDetailProvider
    {
        Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, MemberReadModel creator, string customDetailText,
            DetailTypeEnum detailType, DateTime? startDate, DateTime? endDate = null);
    }
}