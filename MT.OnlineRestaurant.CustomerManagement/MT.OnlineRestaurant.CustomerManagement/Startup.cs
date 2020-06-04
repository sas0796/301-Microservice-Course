using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MT.Online.Restaurant.MessagesManagement.Services;
using MT.OnlineRestaurant.BusinessLayer;
using MT.OnlineRestaurant.DataLayer;
using MT.OnlineRestaurant.DataLayer.Context;
using MT.OnlineRestaurant.Logging;
using MT.OnlineRestaurant.ValidateUserHandler;
using Swashbuckle.AspNetCore.Swagger;

namespace MT.OnlineRestaurant.CustomerManagement
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
            services.AddMvc();
            //Auto Mapper Configurations
           var mappingConfig = new MapperConfiguration(mc =>
           {
               mc.AddProfile(new MapperHelper());
           });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            /*DI*/
            /*DI*/
            services.AddTransient<ICustomerBusiness, CustomerBusiness>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IMessages, Messages>();
            services.AddTransient<ICache, CacheManager>();

            /*Registering connection string*/
            services.Configure<ApplicationString>(Configuration.GetSection("ApplicationString"));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new Info { Title = "CustomerManager", Version = "1.0" });
                c.OperationFilter<HeaderFilter>();
            });
            services.AddDbContext<CustomerManagementContext>(options =>
               options.UseSqlServer("Server=tcp:devserver4.database.windows.net;Initial Catalog=CustomerManagement;User ID=M1043027;Password=Azuredb1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;",
               b => b.MigrationsAssembly("MT.OnlineRestaurant.DataLayer")));
            services.AddMvc()
                    .AddMvcOptions(options =>
                    {
                        options.Filters.Add(new Authorization());
                        //options.Filters.Add(new LoggingFilter(Configuration["ApplicationString:DB"]));
                        //options.Filters.Add(new ErrorHandlingFilter(Configuration["ApplicationString:DB"]));
                        options.MaxModelValidationErrors = 50;
                        options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                            (_) => "The field is required.");
                    });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMessages message)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*middleware to have app response pipeline*/
            app.ExceptionMiddlewareExtensions();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "CustomerManager (V 1.0)");
            });
            app.UseMvc();
            message.RegisterOnMessageHandlerAndReceiveMessages();
        }
    }
}
