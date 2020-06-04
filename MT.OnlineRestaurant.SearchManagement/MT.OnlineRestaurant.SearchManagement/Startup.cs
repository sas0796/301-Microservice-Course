using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MessengerManagement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel;
using MT.OnlineRestaurant.DataLayer.Repository;
using MT.OnlineRestaurant.Logging;
using MT.OnlineRestaurant.ValidateUserHandler;
using Swashbuckle.AspNetCore.Swagger;

namespace MT.OnlineRestaurant.SearchManagement
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
            // services.AddMvc();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "SearchManager", Version = "1.0" });
                c.OperationFilter<HeaderFilter>();
            });

            //services.Configure<ConnectionString>(Configuration.GetSection("ConnectionString"));

            services.AddDbContext<RestaurantManagementContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString"),
               b => b.MigrationsAssembly("MT.OnlineRestaurant.DataLayer")));

            services.AddMvc()
                    .AddMvcOptions(options =>
                    {
                        //options.Filters.Add(new Authorization());
                        //options.Filters.Add(new LoggingFilter(Configuration["ConnectionString:DatabaseConnectionString"]));
                        //options.Filters.Add(new ErrorHandlingFilter(Configuration["ConnectionString:DatabaseConnectionString"]));
                        options.Filters.Add(new LoggingFilter(Configuration.GetConnectionString("DatabaseConnectionString")));
                        options.Filters.Add(new ErrorHandlingFilter(Configuration.GetConnectionString("DatabaseConnectionString")));

                    });

            services.AddTransient<IRestaurantBusiness, RestaurantBusiness>();
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddScoped<IMessenger, Messenger>();

            services.AddScoped<ISubscriptionClient>(x =>
            new SubscriptionClient(Configuration.GetValue<string>("ServiceBus:ConnectionString"),
            Configuration.GetValue<string>("ServiceBus:TopicName"), Configuration.GetValue<string>("ServiceBus:SubscriptionName")));

            var _messenger = services.BuildServiceProvider().GetService<IMessenger>();
            _messenger.ReceiveMessageAsync().Wait();
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
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "SearchManager (V 1.0)");
            });
            app.UseMvc();
            //message.RegisterOnMessageHandlerAndReceiveMessages();
        }
    }
}
