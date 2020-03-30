namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RelationshipEntityBuilder : BaseSqlServerEntityBuilder<Relationship>
    {
        public RelationshipEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<Relationship> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.MemoryBookUniverse)
                    .WithMany(x => x.Relationships)
                    .HasForeignKey(x => x.MemoryBookUniverseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}