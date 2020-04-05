namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Threading.Tasks;
    using Repository.Detail.Models;
    using Repository.Member.Models;
    using Repository.Relationship.Models;

    public interface IRelationshipDetailManager
    {
        Task<DetailReadModel> CreateWedding(
            MemberReadModel creator,
            RelationshipReadModel relationship,
            DateTime weddingDate,
            string weddingLocation = null);
    }
}