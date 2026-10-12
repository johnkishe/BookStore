using BookStore.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("BookStore")));
            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

            services.AddHttpClient<IOpenLibraryService, OpenLibraryService>(client =>
            {
                client.BaseAddress = new Uri("https://openlibrary.org/");
                client.DefaultRequestHeaders.UserAgent.ParseAdd("BookStoreAssessment/1.0");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            return services;
        }
    }
}
