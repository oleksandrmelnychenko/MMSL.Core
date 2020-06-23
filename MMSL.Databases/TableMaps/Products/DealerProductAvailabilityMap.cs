using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace MMSL.Databases.TableMaps.Products {
    public class DealerProductAvailabilityMap : EntityBaseMap<DealerProductAvailability> {
        public override void Map(EntityTypeBuilder<DealerProductAvailability> entity) {
            base.Map(entity);

            entity.ToTable("DealerProductAvailabilities");
        }
    }
}
