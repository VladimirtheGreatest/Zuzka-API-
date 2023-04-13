using Azure.Identity;
using Microsoft.AspNetCore.Hosting;

namespace CodeRama
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           }).ConfigureAppConfiguration((hostContext, builder) =>
           {
               if (hostContext.HostingEnvironment.IsDevelopment())
               {
                   builder.AddUserSecrets("27550aac-9897-406f-a964-fe2551a10660");
               }

               var settings = builder.Build();
               var connectionAppConfig = settings["AppConfig"];

               if (!hostContext.HostingEnvironment.IsDevelopment())
               {
                   connectionAppConfig = settings.GetConnectionString("AzureAppConfiguration");
               }

               //azure app config for production scenarios, connection string should be stored in keyvault.
               //builder.AddAzureAppConfiguration(options =>
               //{
               //    options.Connect(connectionAppConfig)
               //            .ConfigureKeyVault(kv =>
               //            {
               //                kv.SetCredential(new DefaultAzureCredential());
               //            });
               //});
           });
    }
}