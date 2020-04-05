namespace MemoryBook.Desktop
{
    using System;
    using System.Configuration;
    using System.Windows.Forms;
    using DataAccess.Extensions;
    using Microsoft.Extensions.DependencyInjection;

    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ServiceCollection services = new ServiceCollection();

            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];

            services.ConfigureServices();
            services.AddDatabaseContexts(connectionString);

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            Application.Run((MemoryBookForm)serviceProvider.GetService(typeof(MemoryBookForm)));
        }
    }
}