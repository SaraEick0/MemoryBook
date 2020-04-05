namespace MemoryBook.Repository.DetailType.Managers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IDetailTypeQueryManager
    {
        Task<IList<DetailTypeReadModel>> GetAllDetailTypes();

        Task<IList<DetailTypeReadModel>> GetDetailTypes(params DetailTypeEnum[] detailTypes);
    }
}