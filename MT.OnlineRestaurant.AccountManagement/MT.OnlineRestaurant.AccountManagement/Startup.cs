using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.BusinessLayer.Interfaces;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.DataLayer.Interfaces;
using MT.OnlineRestaurant.Logging;
using MT.OnlineRestaurant.ValidateUserHandler;
using MT.OnlineRestuarant.DataLayer;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;

namespace MT.OnlineRestaurant.AccountManagement
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            _applicationPath = env.WebRootPath;
            _contentRootPath = env.ContentRootPath;
            // Setup configuration sources.

            var builder = new ConfigurationBuilder()
                .SetBasePath(_contentRootPath)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            if (env.IsDevelopment())
            {
                // This reads the configuration keys from the secret store.
                // For more details on using the user secret store see http://go.microsoft.com/fwlink/?LinkID=532709
                // builder.AddUserSecrets();
            }
            builder.AddEnvironmentVariables();
             Configuration = builder.Build();
        }
        private static string _applicationPath = string.Empty;
        private static string _contentRootPath = string.Empty;

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "AccountManager", Version = "1.0" });
                c.OperationFilter<HeaderFilter>();
            });

            


            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IUserDataAccess, UserDataAccess>();
            services.Configure<ApplicationString>(Configuration.GetSection("ApplicationString"));
            services.AddMvc(
                          config =>
                          {
                              config.Filters.Add(new LoggingFilter(Configuration.GetConnectionString("DatabaseConnectionString")));
                              config.Filters.Add(new ErrorHandlingFilter(Configuration.GetConnectionString("DatabaseConnectionString")));

                          });
            //addedd
            services.AddDbContext<CustomerManagementContext>(item => item.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString"),
                b=>b.MigrationsAssembly("MT.OnlineRestuarant.DataLayer")));
        
        var appSettingsSection = Configuration.GetSection("AppSettings");
                services.Configure<AppSettings>(appSettingsSection);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "AccountManager (V 1.0)");
            });
            app.UseMvc();

        }
    }
}
