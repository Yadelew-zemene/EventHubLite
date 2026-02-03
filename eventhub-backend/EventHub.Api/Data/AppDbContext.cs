using Microsoft.EntityFrameworkCore;
using EventHub.Api.Entities;
using EventHub.Api.Entities;

namespace EventHub.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
}
