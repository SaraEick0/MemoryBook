namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DetailTypeEntityBuilder : BaseSqlServerEntityBuilder<DetailType>
    {
        public DetailTypeEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<DetailType> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasUniqueCode();
                e.Property(x => x.DetailStartText).IsRequired();
                e.Property(x => x.DetailEndText).IsRequired();
            });
        }
    }
}