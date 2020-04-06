namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Detail.Models;
    using Repository.Member.Models;

    public interface IRelationshipDetailManager
    {
        Task<DetailReadModel> CreateWedding(
            MemberReadModel creator,
            Guid relationshipId,
            DateTime weddingDate,
            string weddingLocation = null);
    }
}