﻿using Microsoft.EntityFrameworkCore;
using MMSL.Databases.TableMaps;
using MMSL.Databases.TableMaps.Identity;
using MMSL.Domain.Entities.Stores;
using MMSL.Domain.Entities.Addresses;
using MMSL.Domain.Entities.Dealer;
using MMSL.Domain.Entities.Identity;
using MMSL.Databases.TableMaps.Dealer;
using MMSL.Databases.TableMaps.Addresses;
using MMSL.Databases.TableMaps.Stores;
using MMSL.Domain.Entities.StoreCustomers;
using MMSL.Databases.TableMaps.StoreCustomers;
using MMSL.Domain.Entities.Options;
using MMSL.Databases.TableMaps.Options;
using MMSL.Domain.Entities.PaymentTypes;
using MMSL.Domain.Entities.CurrencyTypes;
using MMSL.Databases.TableMaps.PaymentTypes;
using MMSL.Databases.TableMaps.CurrencyTypes;
using MMSL.Domain.Entities.Products;
using MMSL.Databases.TableMaps.Products;
using MMSL.Domain.Entities.Measurements;
using MMSL.Databases.TableMaps.Measurements;
using MMSL.Databases.TableMaps.DeliveryTimelines;
using MMSL.Domain.Entities.DeliveryTimelines;
using MMSL.Databases.TableMaps.Fabrics;
using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Databases {
    public class MMSLDbContext : DbContext {       

        public DbSet<Store> Stores { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<OptionUnit> OptionUnits { get; set; }

        public DbSet<UnitValue> UnitValues { get; set; }

        public DbSet<OptionGroup> OptionGroups { get; set; }

        public DbSet<OptionPrice> OptionPrices { get; set; }

        public DbSet<UserIdentityRoleType> UserIdentityRoleTypes { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<UserIdentity> UserIdentities { get; set; }

        public DbSet<DealerAccount> DealerAccounts { get; set; }

        public DbSet<PaymentType> PaymentTypes { get; set; }

        public DbSet<CurrencyType> CurrencyTypes { get; set; }

        public DbSet<StoreCustomer> StoreCustomers { get; set; }

        public DbSet<StoreMapDealerAccount> StoreDealerAccounts { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductCategoryMapOptionGroup> ProductCategoryMapOptionGroups { get; set; }

        public DbSet<Measurement> Measurements { get; set; }

        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }

        public DbSet<MeasurementMapSize> MeasurementMapSizes { get; set; }

        public DbSet<MeasurementSize> MeasurementSizes { get; set; }

        public DbSet<MeasurementMapDefinition> MeasurementMapDefinitions { get; set; }

        public DbSet<MeasurementDefinition> MeasurementDefinitions { get; set; }
        
        public DbSet<MeasurementMapValue> MeasurementMapValues { get; set; }

        public DbSet<FittingType> FittingTypes { get; set; }

        public DbSet<DeliveryTimeline> DeliveryTimelines { get; set; }

        public DbSet<DeliveryTimelineProductMap> DeliveryTimelineProductMaps { get; set; }

        public DbSet<ProductPermissionSettings> ProductPermissionSettings { get; set; }

        public DbSet<PermissionSettings> PermissionSettings { get; set; }

        public DbSet<DealerProductAvailability> DealerProductAvailabilities { get; set; }
        
        public DbSet<DealerMapProductPermissionSettings> DealerMapProductPermissionSettings { get; set; }

        public DbSet<CustomerProfileSizeValue> CustomerProfileSizeValues { get; set; }

        public DbSet<CustomerProfileStyleConfiguration> CustomerProfileStyleConfigurations { get; set; }
        
        public DbSet<CustomerProductProfile> CustomerProductProfiles { get; set; }

        public DbSet<Fabric> Fabrics { get; set; }

        /// <summary>
        ///     ctor().
        /// </summary>
        public MMSLDbContext() { }
       
        public MMSLDbContext(DbContextOptions<MMSLDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            optionsBuilder.UseSqlServer("Data Source=78.152.175.67;Initial Catalog=MMSL_Identity;Integrated Security=False;User ID=ef_migrator;Password=Grimm_jow92;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.AddConfiguration(new StoreMap());
            modelBuilder.AddConfiguration(new AddressMap());
            modelBuilder.AddConfiguration(new OptionUnitMap());
            modelBuilder.AddConfiguration(new UnitValueMap());
            modelBuilder.AddConfiguration(new OptionGroupMap());
            modelBuilder.AddConfiguration(new OptionPriceMap());
            modelBuilder.AddConfiguration(new UserIdentityRoleTypeMap());
            modelBuilder.AddConfiguration(new UserRoleMap());
            modelBuilder.AddConfiguration(new UserIdentityMap());
            modelBuilder.AddConfiguration(new DealerAccountMap());
            modelBuilder.AddConfiguration(new PaymentTypeMap());
            modelBuilder.AddConfiguration(new CurrencyTypeMap());
            modelBuilder.AddConfiguration(new StoreCustomersMap());
            modelBuilder.AddConfiguration(new StoreDealerAccountMap());
            modelBuilder.AddConfiguration(new ProductCategoryMap());
            modelBuilder.AddConfiguration(new ProductCategoryMapOptionGroupMap());
            modelBuilder.AddConfiguration(new MeasurementMap());
            modelBuilder.AddConfiguration(new MeasurementUnitMap());
            modelBuilder.AddConfiguration(new MeasurementDefinitionMap());
            modelBuilder.AddConfiguration(new MeasurementMapDefinitionMap());
            modelBuilder.AddConfiguration(new MeasurementSizeMap());
            modelBuilder.AddConfiguration(new MeasurementMapSizeMap());
            modelBuilder.AddConfiguration(new MeasurementMapValueMap());
            modelBuilder.AddConfiguration(new FittingTypeMap());
            modelBuilder.AddConfiguration(new DeliveryTimelineMap());
            modelBuilder.AddConfiguration(new DeliveryTimelineProductMapMap());
            modelBuilder.AddConfiguration(new ProductPermissionSettingsMap());
            modelBuilder.AddConfiguration(new PermissionSettingsMap());
            modelBuilder.AddConfiguration(new DealerProductAvailabilityMap());
            modelBuilder.AddConfiguration(new DealerMapProductPermissionSettingsMap());
            modelBuilder.AddConfiguration(new CustomerProfileSizeValueMap());
            modelBuilder.AddConfiguration(new CustomerProductProfileMap());
            modelBuilder.AddConfiguration(new CustomerProfileStyleConfigurationMap());
            modelBuilder.AddConfiguration(new FabricMap());
        }
    }
}