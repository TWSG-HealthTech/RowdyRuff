using System.IO;
using Microsoft.AspNetCore.Hosting;
using RowdyRuff.Repository;

namespace RowdyRuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            var hostingEnvironment = Resolve<IHostingEnvironment>(host);
            if (hostingEnvironment.IsDevelopment())
            {
                var dbContext = Resolve<RowdyRuffContext>(host);
                DbInitializer.Seed(dbContext);
            }

            host.Run();
        }

        private static T Resolve<T>(IWebHost host)
        {
            return (T)host.Services.GetService(typeof(T));
        }
    }
}
