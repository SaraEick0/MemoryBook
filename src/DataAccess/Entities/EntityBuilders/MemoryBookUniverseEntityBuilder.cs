namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MemoryBookUniverseEntityBuilder : BaseSqlServerEntityBuilder<MemoryBookUniverse>
    {
        public MemoryBookUniverseEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<MemoryBookUniverse> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasUniqueName();
            });
        }
    }
}