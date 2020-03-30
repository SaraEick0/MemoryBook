namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RelationshipMembershipEntityBuilder : BaseSqlServerEntityBuilder<RelationshipMembership>
    {
        public RelationshipMembershipEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        protected override void Configure(EntityTypeBuilder<RelationshipMembership> typeBuilder)
        {
            typeBuilder.Has(e =>
            {
                e.HasOne(x => x.Member)
                    .WithMany(x => x.RelationshipMemberships)
                    .HasForeignKey(x => x.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.MemberRelationshipType)
                    .WithMany(x => x.RelationshipMemberships)
                    .HasForeignKey(x => x.MemberRelationshipTypeId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasOne(x => x.Relationship)
                    .WithMany(x => x.RelationshipMemberships)
                    .HasForeignKey(x => x.RelationshipId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}