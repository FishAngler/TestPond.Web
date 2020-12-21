using System.Globalization;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestPond.BusinessLayer.Models;
using TestPond.BusinessLayer.Repositories;
using TestPond.BusinessLayer.Services.CollectionRun;
using TestPond.BusinessLayer.Services.NUnitDeserialization;
using TestPond.WebAPI;

namespace TestPond.Web
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
            /////// DATA
            string connStr = Configuration["ConnectionStrings:Default"];

            // DBContext
            services.AddDbContext<TestPondContext>(options => options.UseSqlServer(connStr)
            .EnableSensitiveDataLogging().EnableDetailedErrors(), ServiceLifetime.Scoped);

            // Repository
            services.AddScoped<TestPondRepository>();

            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(typeof(CollectionRunService).GetTypeInfo().Assembly);

            /////// SERVICES
            // Collection Run
            services.AddScoped<CollectionRunService>();
            // NUnit
            services.AddTransient<INUnitXMLDeserializer, NUnitXMLDeserializer>();

            ///// WEB
            services.AddControllers().AddNewtonsoftJson();
            services.AddRazorPages();

            //Utilities
            services.AddTransient<IJsonService, JsonService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var cultureInfo = new CultureInfo("en-US");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }

        public void ConfigureWithSqlLite()
        {
            //In-Memory SQLite
            //services.AddDbContext<TestPondContext>(options => options.UseInMemoryDatabase("TestPondDb")
            //.EnableSensitiveDataLogging().EnableDetailedErrors(), ServiceLifetime.Singleton);

            //Physical SQLite File - Singleton
            //services.AddEntityFrameworkSqlite().AddDbContext<TestPondContext>(options =>
            //options.EnableSensitiveDataLogging().EnableDetailedErrors(), ServiceLifetime.Singleton);

            //Physical SQLite File - Scoped
            //services.AddEntityFrameworkSqlite().AddDbContext<TestPondContext>(options =>
            //options.EnableSensitiveDataLogging().EnableDetailedErrors(), ServiceLifetime.Scoped);
        }
    }
}
