using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using FluentValidation.AspNetCore;

namespace WebApi
{
    public class Startup
    {
        // CONSTRUCTORS
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // PROPERTIES
        public IConfiguration Configuration { get; }

        // METHODS
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IConfiguration>(provider => Configuration);

            DataAccessLayer.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);
            BusinessLayer.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);
            
            
            // Cors
            services.AddCors();
            
            services.AddMvc(options =>
            {
                options.Filters.Add<Filters.ValidationActionFilterAttribute>();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddFluentValidation(conf => conf.RegisterValidatorsFromAssemblyContaining<Startup>());

            // for fluent validation, to not run before filter
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Cors
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseHttpsRedirection();
            app.UseMvc();
            
        }
    }
}
