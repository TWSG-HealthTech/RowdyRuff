using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RowdyRuff.Core.Common;
using RowdyRuff.Repository.Common;

namespace RowdyRuff.Repository
{
    public class Bootstrapper
    {
        private readonly IConfigurationRoot _configuration;

        public Bootstrapper(IConfigurationRoot configurationRoot)
        {
            _configuration = configurationRoot;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<RowdyRuffContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IClientProfileRepository, ClientProfileRepository>();
        }

        public void Configure()
        {
        }

        public void ConfigureDevelopment(RowdyRuffContext dbContext)
        {
            DbInitializer.Seed(dbContext);
        }
    }
}
