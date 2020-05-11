using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MMSL.Databases.TableMaps
{
    public abstract class EntityTypeConfiguration<TEntity> where TEntity : class
    {
        public abstract void Map(EntityTypeBuilder<TEntity> builder);
    }
}
