using Microsoft.EntityFrameworkCore;
using SignIn.Api.Database.DbModel;
using SignIn.Api.Models;

namespace SignIn.Api.Database.EF
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
    public DbSet<ClsUser> Users { get; set; }
  }
}
