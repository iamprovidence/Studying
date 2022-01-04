using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using AutoMapper;

namespace BusinessLayer.Infrastructure
{
    public static class AspInfrastructure
    {

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // add services
            services.AddScoped(typeof(Interfaces.ICommandProcessor), typeof(Commands.CommandProcessor));
            services.AddScoped(typeof(Interfaces.IQueryProcessor), typeof(Queries.QueryProcessor));

            services.AddScoped<IMapper>(f => new MapperConfig().BuildConfiguration().CreateMapper());
        }
    }
}
