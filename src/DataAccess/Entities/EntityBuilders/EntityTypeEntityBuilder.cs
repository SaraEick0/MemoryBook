namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EntityTypeEntityBuilder : BaseSqlServerEntityBuilder<EntityType>
    {
        public EntityTypeEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<EntityType> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasUniqueCode();
            });
        }
    }
}