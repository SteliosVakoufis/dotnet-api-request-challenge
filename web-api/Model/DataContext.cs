using Microsoft.EntityFrameworkCore;

namespace web_api.Model
{
    public class DataContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DataContext(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server with connection string from app settings
            options.UseSqlServer(Configuration.GetConnectionString("WebApiDatabase"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IPInfoEntity>()
                .HasKey(entity => entity.IP);
        }

        public DbSet<IPInfoEntity> IPInfo { get; set; }
    }
}
