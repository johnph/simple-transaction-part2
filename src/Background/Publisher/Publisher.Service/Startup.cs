namespace Publisher.Service
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Publisher.Framework.Extensions;
    using Publisher.Service.Publishers;

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
            services.AddCommandConfiguration(Configuration);
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, ScheduleTask>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    internal static class ServiceExtensions
    {
        public static IServiceCollection AddCommandConfiguration(this IServiceCollection services, IConfiguration config)
        {
            // Command Config
            services.AddSingletonCommandPublisher(config);
            return services;
        }
    }
}
