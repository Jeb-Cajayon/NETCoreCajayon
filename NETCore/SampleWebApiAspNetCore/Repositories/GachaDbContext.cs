using Microsoft.EntityFrameworkCore;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Repositories
{
    public class GachaDbContext : DbContext
    {
        public GachaDbContext(DbContextOptions<GachaDbContext> options)
            : base(options)
        {
        }

        public DbSet<GachaEntity> GachaItems { get; set; } = null!;
    }
}
