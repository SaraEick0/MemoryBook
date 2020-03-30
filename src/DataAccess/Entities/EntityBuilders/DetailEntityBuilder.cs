namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DetailEntityBuilder : BaseSqlServerEntityBuilder<Detail>
    {
        public DetailEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<Detail> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.Creator)
                 .WithMany(x => x.CreatedDetails)
                 .HasForeignKey(x => x.CreatorId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(x => x.DetailType)
                 .WithMany(x => x.Details)
                 .HasForeignKey(x => x.DetailTypeId)
                 .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}