using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Dealer;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Databases.TableMaps.Dealer {
    class DealerMapProductPermissionSettingsMap : EntityBaseMap<DealerMapProductPermissionSettings> {
        public override void Map(EntityTypeBuilder<DealerMapProductPermissionSettings> entity) {
            base.Map(entity);

            entity.ToTable("DealerMapProductPermissionSettingsMap");

            entity
                .HasOne(x => x.DealerAccount)
                .WithMany(x => x.DealerMapProductPermissionSettings)
                .HasForeignKey(x => x.DealerAccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity
                .HasOne(x => x.ProductPermissionSettings)
                .WithMany(x => x.DealerMapProductPermissionSettings)
                .HasForeignKey(x => x.ProductPermissionSettingsId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
