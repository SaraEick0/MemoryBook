namespace MemoryBook.Repository.Detail.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.Group.Models;
    using Business.Member.Models;
    using Business.Relationship.Models;

    public interface IDetailAssociationProvider
    {
        Task CreateDetailAssociation(DetailReadModel detail, IList<MemberReadModel> members);

        Task CreateDetailAssociation(DetailReadModel detail, RelationshipReadModel relationship);

        Task CreateDetailAssociation(DetailReadModel detail, GroupReadModel group);
    }
}