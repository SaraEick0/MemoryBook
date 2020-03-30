namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MemberEntityBuilder : BaseSqlServerEntityBuilder<Member>
    {
        public MemberEntityBuilder(ModelBuilder modelBuilder)
          : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<Member> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.Property(x => x.FirstName).IsRequired();
                e.Property(x => x.LastName).IsRequired();

                e.HasOne(x => x.MemoryBookUniverse)
                    .WithMany(x => x.Members)
                    .HasForeignKey(x => x.MemoryBookUniverseId);
            });
        }
    }
}