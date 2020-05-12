using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.BankDetails;

namespace MMSL.Databases.TableMaps.BankDetails {
    public class BankDetailMap : EntityBaseMap<BankDetail> {
        public override void Map(EntityTypeBuilder<BankDetail> entity) {
            base.Map(entity);
            entity.ToTable("BankDetails");
        }
    }
}
