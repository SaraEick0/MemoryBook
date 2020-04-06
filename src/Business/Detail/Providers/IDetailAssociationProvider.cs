namespace MemoryBook.Business.Detail.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Detail.Models;

    public interface IDetailAssociationProvider
    {
        Task CreateMemberDetailAssociation(DetailReadModel detail, IList<Guid> memberIds);

        Task CreateRelationshipDetailAssociation(DetailReadModel detail, Guid relationshipId);

        Task CreateGroupDetailAssociation(DetailReadModel detail, Guid groupId);
    }
}