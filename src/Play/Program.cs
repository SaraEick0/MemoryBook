namespace Play
{
    using System;
    using System.Configuration;
    using System.Windows.Forms;
    using Microsoft.Extensions.DependencyInjection;

    static class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// The main entry point for the application.
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

            ServiceProvider = services.BuildServiceProvider();

            Application.Run((Form1)ServiceProvider.GetService(typeof(Form1)));
        }
    }
}