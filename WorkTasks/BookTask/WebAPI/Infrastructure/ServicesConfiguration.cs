using AutoMapper;

using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;

using DataAccessLayer.Interfaces;
using DataAccessLayer.Repositories;

using FluentValidation.AspNetCore;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Swashbuckle.AspNetCore.Swagger;

namespace WebAPI.Infrastructure
{
    internal static class ServicesConfiguration
    {
        public static void AddMapper(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(typeof(Domain.MappingProfiles.BookProfile).Assembly);
        }

        public static void AddDataAccessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IBookRepository, BookRepository>();
        }

        public static void AddBusinessLogicServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBookService, BookService>();
        }

        #region Validation
        public static void AddFluentValidation(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder.AddFluentValidation(conf => conf.RegisterValidatorsFromAssemblyContaining<Domain.Validation.Book.CreateBookDTOValidator>());
        }

        public static void AddValidation(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }
        #endregion

        #region Swagger
        public static void AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(conf =>
            {
                conf.SwaggerDoc(name: "v1", info: new Info() { Title = "BookApi", Version = "v1" });
            });
        }

        public static void UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(opt =>
            {
                opt.RouteTemplate = "swagger/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint(url: "v1/swagger.json", name: "Book Api");
            }); 
        }
        #endregion
    }
}
