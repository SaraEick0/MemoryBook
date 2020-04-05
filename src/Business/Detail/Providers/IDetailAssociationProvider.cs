namespace MemoryBook.Business.Detail.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Detail.Models;
    using Repository.Group.Models;
    using Repository.Member.Models;
    using Repository.Relationship.Models;

    public interface IDetailAssociationProvider
    {
        Task CreateDetailAssociation(DetailReadModel detail, IList<MemberReadModel> members);

        Task CreateDetailAssociation(DetailReadModel detail, RelationshipReadModel relationship);

        Task CreateDetailAssociation(DetailReadModel detail, GroupReadModel group);
    }
}