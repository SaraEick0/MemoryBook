namespace MemoryBook.Repository.Detail.Providers
{
    using System;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.DetailType;
    using Business.Member.Models;

    public interface IDetailProvider
    {
        Task<DetailReadModel> CreateDetail(Guid memoryBookUniverseId, MemberReadModel creator, string customDetailText,
            DetailTypeEnum detailType, DateTime? startDate, DateTime? endDate = null);
    }
}