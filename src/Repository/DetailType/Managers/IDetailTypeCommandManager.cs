namespace MemoryBook.Repository.DetailType.Managers
{
    using System;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailTypeCommandManager
    {
        Task CreateDetailType(params DetailTypeCreateModel[] models);

        Task UpdateDetailType(params DetailTypeUpdateModel[] models);

        Task DeleteDetailType(params Guid[] groupIds);
    }
}