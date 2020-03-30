namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class DetailPermissionEntityBuilder : BaseSqlServerEntityBuilder<DetailPermission>
    {
        public DetailPermissionEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<DetailPermission> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.Detail)
                    .WithMany(x => x.Permissions)
                    .HasForeignKey(x => x.DetailId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Member)
                    .WithMany(x => x.Permissions)
                    .HasForeignKey(x => x.MemberId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}