namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RelationshipTypeEntityBuilder : BaseSqlServerEntityBuilder<RelationshipType>
    {
        public RelationshipTypeEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<RelationshipType> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasUniqueCode();
            });
        }
    }
}