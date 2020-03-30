namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.Member.Models;
    using Business.Relationship.Models;

    public interface IRelationshipDetailManager
    {
        Task<DetailReadModel> CreateWedding(
            MemberReadModel creator,
            RelationshipReadModel relationship,
            DateTime weddingDate,
            string weddingLocation = null);
    }
}