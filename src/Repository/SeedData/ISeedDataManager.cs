namespace MemoryBook.Repository
{
    using System.Threading.Tasks;

    public interface ISeedDataManager
    {
        Task LoadSeedData();
    }
}