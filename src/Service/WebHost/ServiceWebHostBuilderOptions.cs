namespace MemoryBook.Service.WebHost
{
    using Configuration;
    using Microsoft.Extensions.Configuration;

    public class ServiceWebHostBuilderOptions
    {
        private string[] arguments;

        public IConfiguration Configuration { get; set; }

        public ApplicationOptions Options { get; set; }

        public bool EnableConsoleLogging { get; set; }

        public string[] GetArguments()
        {
            return this.arguments;
        }

        public void SetArguments(string[] value)
        {
            this.arguments = value;
        }
    }
}