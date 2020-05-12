﻿using Microsoft.EntityFrameworkCore;
using MMSL.Databases.TableMaps;
using MMSL.Databases.TableMaps.BankDetails;
using MMSL.Databases.TableMaps.Identity;
using MMSL.Domain.Entities.BankDetails;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Databases {
    public class MMSLDbContext : DbContext {
        public MMSLDbContext() { }

        public DbSet<UserIdentity> UserIdentities { get; set; }
        public DbSet<DealerAccount> DealerAccounts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public DbSet<BankDetail> BankDetails { get; set; }

        public MMSLDbContext(
            DbContextOptions<MMSLDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=78.152.175.67;Initial Catalog=Mmsl;Integrated Security=False;User ID=ef_migrator;Password=Grimm_jow92;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddConfiguration(new UserIdentityMap());
            modelBuilder.AddConfiguration(new BankDetailMap());
        }
    }
}