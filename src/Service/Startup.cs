namespace MemoryBook.Service
{
    using System;
    using Common.Extensions;
    using DataAccess.Extensions;
    using MemoryBook.Service.Configuration;
    using MemoryBook.Service.SwaggerGeneration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Security;
    using Swashbuckle.AspNetCore.SwaggerUI;

    public class Startup
    {
        private readonly ApplicationConfiguration applicationConfiguration;

        public Startup(IOptions<ApplicationConfiguration> applicationConfiguration)
        {
            Contract.RequiresNotNull(applicationConfiguration, nameof(applicationConfiguration));

            this.applicationConfiguration = applicationConfiguration.Value;
        }

        private ApplicationOptions ApplicationOptions => this.applicationConfiguration.Application;

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (this.ApplicationOptions.UseSwagger)
            {
                app.UseSwagger();
                string baseUrl = this.ApplicationOptions.BindUrl;
                app.UseSwaggerUI((Action<SwaggerUIOptions>)(c =>
                {
                        string lowerInvariant = new Uri((baseUrl + "swagger/doc/swagger.json").ToLowerInvariant()).ToString();
                        c.SwaggerEndpoint(lowerInvariant, "Memory Book API");
                }));
            }

            app.UseResponseCompression();

            app.UseXContentTypeOptions();
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddResponseCompression();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });

            services.AddRouting();

            if (this.ApplicationOptions.UseSwagger)
            {
                services.AddSwaggerGeneration();
            }

            services.AddDatabaseContexts(this.ApplicationOptions.ConnectionString, ServiceLifetime.Scoped);

            services.AddSecurity();

            services.AddManagers();
        }
    }
}