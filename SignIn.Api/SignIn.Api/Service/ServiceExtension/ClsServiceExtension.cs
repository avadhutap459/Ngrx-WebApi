using Microsoft.EntityFrameworkCore;
using SignIn.Api.Database.EF;
using SignIn.Api.Service.Interface;
using SignIn.Api.Service.Services;
using SignIn.Api.Service.UnitOfWork;

namespace SignIn.Api.Service.ServiceExtension
{
  public static class ClsServiceExtension
  {
    public static IServiceCollection AddDIServices(this IServiceCollection services , IConfiguration configuration)
    {
      services.AddDbContext<AppDbContext>(op =>
      {
        op.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
      });

      services.AddScoped<IUnitOfWork, ClsUnitOfWork>();

      return services;
    }
  }
}
