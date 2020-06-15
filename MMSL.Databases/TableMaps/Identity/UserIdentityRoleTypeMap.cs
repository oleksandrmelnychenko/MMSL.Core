using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Databases.TableMaps.Identity {
    public class UserIdentityRoleTypeMap : EntityBaseMap<UserIdentityRoleType> {
        public override void Map(EntityTypeBuilder<UserIdentityRoleType> entity) {
            base.Map(entity);

            entity.ToTable("UserIdentityRoleTypes");
        }
    }
}
