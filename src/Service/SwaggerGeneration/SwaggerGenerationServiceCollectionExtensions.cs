namespace MemoryBook.Service.SwaggerGeneration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.Extensions;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public static class SwaggerGenerationServiceCollectionExtensions
    {
        public static IServiceCollection AddSwaggerGeneration(this IServiceCollection services)
        {
            Contract.RequiresNotNull(services, nameof(services));

            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();

                options.SwaggerDoc("doc", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Memory Book Service"
                });

                options.AddDocFiles("MemoryBook.Service.xml", "MemoryBook.Business.xml");

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

            return services;
        }
        private static SwaggerGenOptions AddDocFiles(
            this SwaggerGenOptions options,
            params string[] documentationFileNames)
        {
            foreach (string documentationFileName in documentationFileNames)
            {
                string str = Path.Combine(AppContext.BaseDirectory, documentationFileName);
                if (File.Exists(str))
                    options.IncludeXmlComments(str, false);
            }
            return options;
        }

    }
}