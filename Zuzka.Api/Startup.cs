using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Zuzka.Data;
using Zuzka.Repository;
using Zuzka.RepositoryContracts;
using Zuzka.Services;
using Zuzka.Services.Contracts;


namespace CodeRama
{
    public class Startup
    {
        private string? _connection = null;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //DI
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IValidationService, ValidationService>();

            services.AddAutoMapper(typeof(Startup));

            //only local environment, for production get the value from appconfig/keyvault;
            _connection = Configuration.GetConnectionString("Default");


            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Zuzka API",
                    Description = "Zuzka API",
                });
            });

            //EF
            services.AddDbContext<DocumentContext>(options =>
            {
                options.UseSqlServer(_connection, providerOptions => providerOptions.EnableRetryOnFailure());
            });

            // services.BuildServiceProvider().GetService<DocumentContext>().Database.Migrate(); //when deploying to production

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
            }).AddXmlSerializerFormatters();

            services.AddMemoryCache();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            //azure app config for production scenarios
            //services.AddAzureAppConfiguration();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();
            app.UseSwaggerUI(x => {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "ZuzkaAPI");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
