namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class GroupMembershipEntityBuilder : BaseSqlServerEntityBuilder<GroupMembership>
    {
        public GroupMembershipEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<GroupMembership> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.Group)
                    .WithMany(x => x.GroupMemberships)
                    .HasForeignKey(x => x.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Member)
                    .WithMany(x => x.GroupMemberships)
                    .HasForeignKey(x => x.MemberId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}