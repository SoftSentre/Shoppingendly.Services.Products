using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework;
using Shoppingendly.Services.Products.Infrastructure.EntityFramework.Converters;

namespace Shoppingendly.Services.Products.WebApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEntityFrameworkSqlServer()
                .AddDbContext<ProductServiceDbContext>(options =>
                    {
                        options.UseSqlServer(_configuration["ConnectionString"],
                            sqlOptions =>
                            {
                                sqlOptions.MigrationsAssembly(typeof(ProductServiceDbContext).GetTypeInfo().Assembly
                                    .GetName().Name);
                                sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                            });
                        options.ReplaceService<IValueConverterSelector, StronglyTypedIdValueConverterSelector>();
                    },
                    ServiceLifetime.Scoped
                );
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}