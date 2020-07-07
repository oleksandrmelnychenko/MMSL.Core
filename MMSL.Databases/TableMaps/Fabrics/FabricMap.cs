using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Fabrics;

namespace MMSL.Databases.TableMaps.Fabrics {
    public class FabricMap : EntityBaseMap<Fabric> {
        public override void Map(EntityTypeBuilder<Fabric> entity) {
            base.Map(entity);

            entity.ToTable("Fabrics");
        }
    }
}
