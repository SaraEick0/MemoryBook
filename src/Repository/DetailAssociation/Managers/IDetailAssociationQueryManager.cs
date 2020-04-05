namespace MemoryBook.Repository.DetailAssociation.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailAssociationQueryManager
    {
        Task<IList<DetailAssociationReadModel>> GetDetailAssociationByDetailId(params Guid[] detailIds);

        Task<IList<DetailAssociationReadModel>> GetDetailAssociationByMemberId(params Guid[] memberIds);

        Task<IList<DetailAssociationReadModel>> GetDetailAssociationByGroupId(params Guid[] groupIds);

        Task<IList<DetailAssociationReadModel>> GetDetailAssociationByRelationshipId(params Guid[] relationshipIds);
    }
}