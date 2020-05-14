using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.PaymentTypes;

namespace MMSL.Databases.TableMaps.PaymentTypes {
    public class PaymentTypeMap : EntityBaseMap<PaymentType> {
        public override void Map(EntityTypeBuilder<PaymentType> entity) {
            base.Map(entity);

            entity.ToTable("PaymentTypes");
        }
    }
}
