using Microsoft.EntityFrameworkCore;
using EventHub.Api.Entities.User;

namespace EventHub.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
}
