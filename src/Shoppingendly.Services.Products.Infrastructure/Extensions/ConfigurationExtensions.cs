using Microsoft.Extensions.Configuration;

namespace Shoppingendly.Services.Products.Infrastructure.Extensions
{
    public static class ConfigurationExtensions
    {
        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }
    }
}