using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ParkingSystem;
using ParkingSystem.Interfaces;
using static ParkingSystem.Core.Configurations;

namespace ParkingWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSingleton<Parking>();
            services.AddSingleton<IParkingPlace, ParkingSystem.ParkingPlace.DefaultParkingPlace>();
            services.AddSingleton<ITimeService, ParkingSystem.TimeServices.DefaultTimeService>(factory =>
                new ParkingSystem.TimeServices.DefaultTimeService(PAYMENT_FREQUENCY_IN_SECOND, SAVE_COMMIT_TIME_FREQUENCY_IN_MINUTE));
            services.AddSingleton<ITransactionService, ParkingSystem.TransactionServices.DefaultTransactionService>();
            services.AddSingleton<IVehicleInitializer, ParkingSystem.VehicleInitializers.DefaultVehicleInitializer>();
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
        }
    }
}
