namespace MemoryBook.Service.Configuration
{
    using Microsoft.Extensions.Configuration;

    public interface IApplicationConfigurationProvider
    {
        IConfiguration GetConfiguration(string[] args);
    }
}