using Microsoft.EntityFrameworkCore;
using MMSL.Common;
using MMSL.Databases.TableMaps;
using MMSL.Databases.TableMaps.Identity;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Databases {
    public class MMSLDbContext : DbContext {
        public MMSLDbContext() { }

        public DbSet<UserIdentity> UserIdentities { get; set; }

        public MMSLDbContext(
            DbContextOptions<MMSLDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer(ConfigurationManager.DatabaseConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddConfiguration(new UserIdentityMap());
        }
    }
}