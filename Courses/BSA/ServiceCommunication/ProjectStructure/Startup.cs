using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectStructure
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
            // RabbitMQ
            QueueService.Infrastructure.AspInfrastructure.ConfigureServices(services, Configuration);

            // service, which asynchronous checks if FileWorker
            // had enqueue some data
            services.AddHostedService<HostedServices.MessageService>();
            services.AddSignalR();

            services.AddMvc(options =>
            {
                // filter, that before each methods
                // enqueues Controller and Action's names for File Worker
                options.Filters.Add<Filters.ExecutedActionFilterAttribute>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSignalR(routes =>
            {
                routes.MapHub<Hubs.MessageHub>("/message");
            });
        }
    }
}
