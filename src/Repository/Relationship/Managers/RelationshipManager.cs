namespace MemoryBook.Repository.Relationship.Managers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Member.Models;
    using Business.Relationship.Managers;
    using Business.Relationship.Models;
    using Business.RelationshipMembership.Managers;
    using Business.RelationshipMembership.Models;
    using Business.RelationshipType;
    using Business.RelationshipType.Managers;

    public class RelationshipManager : IRelationshipManager
    {
        private readonly IRelationshipCommandManager relationshipCommandManager;
        private readonly IRelationshipMembershipCommandManager relationshipMembershipCommandManager;
        private readonly IRelationshipTypeQueryManager relationshipTypeQueryManager;

        public RelationshipManager(IRelationshipCommandManager relationshipCommandManager, IRelationshipMembershipCommandManager relationshipMembershipCommandManager, IRelationshipTypeQueryManager relationshipTypeQueryManager)
        {
            this.relationshipCommandManager = relationshipCommandManager ?? throw new ArgumentNullException(nameof(relationshipCommandManager));
            this.relationshipMembershipCommandManager = relationshipMembershipCommandManager ?? throw new ArgumentNullException(nameof(relationshipMembershipCommandManager));
            this.relationshipTypeQueryManager = relationshipTypeQueryManager ?? throw new ArgumentNullException(nameof(relationshipTypeQueryManager));
        }

        public async Task CreateRelationship(MemberReadModel firstMember, MemberReadModel secondMember, RelationshipTypeEnum firstMemberRelationshipType, RelationshipTypeEnum secondMemberRelationshipType, DateTime? startDate, DateTime? endDate)
        {
            if (firstMember == null)
            {
                throw new ArgumentNullException(nameof(firstMember));
            }
            if (secondMember == null)
            {
                throw new ArgumentNullException(nameof(secondMember));
            }

            var relationshipTypes = await this.relationshipTypeQueryManager.GetRelationshipTypes(firstMemberRelationshipType, secondMemberRelationshipType)
                .ConfigureAwait(false);

            if (relationshipTypes == null || relationshipTypes.Count < 2)
            {
                return;
            }

            var firstMemberRelationshipTypeId = relationshipTypes.First(x => x.Code.Equals(firstMemberRelationshipType.ToString())).Id;
            var secondMemberRelationshipTypeId = relationshipTypes.First(x => x.Code.Equals(secondMemberRelationshipType.ToString())).Id;


            var relationship = new RelationshipCreateModel
            {
                StartDate = startDate,
                EndDate = endDate
            };

            var relationshipId = await this.relationshipCommandManager.CreateRelationship(firstMember.MemoryBookUniverseId, relationship)
                .ConfigureAwait(false);


            var firstRelationshipMembership = new RelationshipMembershipCreateModel
            {
                StartDate = startDate,
                EndDate = endDate,
                MemberId = firstMember.Id,
                MemberRelationshipTypeId = firstMemberRelationshipTypeId,
                RelationshipId = relationshipId.FirstOrDefault()
            };

            var secondRelationshipMembership = new RelationshipMembershipCreateModel
            {
                StartDate = startDate,
                EndDate = endDate,
                MemberId = secondMember.Id,
                MemberRelationshipTypeId = secondMemberRelationshipTypeId,
                RelationshipId = relationshipId.FirstOrDefault()
            };

            await this.relationshipMembershipCommandManager
                .CreateRelationshipMembership(firstRelationshipMembership, secondRelationshipMembership)
                .ConfigureAwait(false);
        }
    }
}