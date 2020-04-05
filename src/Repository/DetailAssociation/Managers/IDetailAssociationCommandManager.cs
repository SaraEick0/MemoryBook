namespace MemoryBook.Repository.DetailAssociation.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailAssociationCommandManager
    {
        Task<IList<Guid>> CreateDetailAssociation(params DetailAssociationCreateModel[] models);

        Task DeleteDetailAssociations(params Guid[] detailAssociationIds);
    }
}