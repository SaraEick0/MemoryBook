namespace MemoryBook.Service.WebHost
{
    using Microsoft.AspNetCore.Hosting;

    public interface IServiceWebHostBuilder
    {
        IWebHost Build(ServiceWebHostBuilderOptions webHostBuilderOptions);
    }
}