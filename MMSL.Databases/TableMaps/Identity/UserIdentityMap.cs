using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MMSL.Domain.Entities.Identity;

namespace MMSL.Databases.TableMaps.Identity
{
    public class UserIdentityMap : EntityBaseMap<UserIdentity>
    {
        public override void Map(EntityTypeBuilder<UserIdentity> entity)
        {
            base.Map(entity);
            entity.ToTable("UserIdentities");
            entity
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
