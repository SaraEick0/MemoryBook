namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DetailAssociationEntityBuilder : BaseSqlServerEntityBuilder<DetailAssociation>
    {
        public DetailAssociationEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<DetailAssociation> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.Detail)
                    .WithMany(x => x.DetailAssociations)
                    .HasForeignKey(x => x.DetailId)
                    .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.EntityType)
                    .WithMany(x => x.DetailAssociations)
                    .HasForeignKey(x => x.EntityTypeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}