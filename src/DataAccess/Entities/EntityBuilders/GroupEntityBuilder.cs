namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GroupEntityBuilder : BaseSqlServerEntityBuilder<Group>
    {
        public GroupEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<Group> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasUniqueCode();
                e.HasUniqueName();

                e.HasOne(x => x.MemoryBookUniverse)
                    .WithMany(x => x.Groups)
                    .HasForeignKey(x => x.MemoryBookUniverseId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}