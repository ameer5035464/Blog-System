using BlogSystem.DAL._Data;
using BlogSystem.DAL._Identity;
using BlogSystem.DAL.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlogSystem.DAL
{
    public static class DependencyInjection
    {
        public static IServiceCollection DALServices(this IServiceCollection services, IConfiguration configuration)
        {

            #region Data
            services.AddDbContext<BlogSystemDbContext>(options =>
              {
                  options
                  .UseLazyLoadingProxies()
                  .UseSqlServer(configuration.GetConnectionString("BlogSystemDB"));
              });

            services.AddScoped(typeof(IDbDataIntializer), typeof(DataIntialize));
            #endregion

            #region Identity
            services.AddDbContext<BlogIdentityDbContext>(options =>
              {
                  options
                 .UseLazyLoadingProxies()
                 .UseSqlServer(configuration.GetConnectionString("BlogSystemIdentityDB"));

              });
            services.AddScoped(typeof(IDbIdentityIntializer), typeof(IdentityIntialize));
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            #endregion


            return services;
        }
    }
}
