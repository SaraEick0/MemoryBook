namespace MemoryBook.Business.SeedData
{
    using System.Threading.Tasks;

    public interface ISeedDataManager
    {
        Task LoadSeedData();
    }
}