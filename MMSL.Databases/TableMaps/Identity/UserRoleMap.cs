using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Databases.TableMaps.Identity {
    public class UserRoleMap : EntityBaseMap<UserRole> {
        public override void Map(EntityTypeBuilder<UserRole> entity) {
            base.Map(entity);

            entity.ToTable("UserRoles");
        }
    }
}
