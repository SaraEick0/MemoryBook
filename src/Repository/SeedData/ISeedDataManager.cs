namespace MemoryBook.Repository.SeedData
{
    using System.Threading.Tasks;

    public interface ISeedDataManager
    {
        Task LoadSeedData();
    }
}