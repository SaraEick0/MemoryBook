namespace MemoryBook.Service.Configuration
{
    /// <summary>
    /// Application options.
    /// </summary>
    public class ApplicationOptions
    {
        public static string ConfigurationKey => "Application";

        public string ConnectionString { get; set; }

        public bool UseSwagger { get; set; }

        public string BindUrl { get; set; }

        public string LogFolder { get; set; }
    }
}