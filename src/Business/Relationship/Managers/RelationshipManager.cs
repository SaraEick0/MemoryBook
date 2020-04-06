namespace MemoryBook.Business.Relationship.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Providers;
    using Relationship.Models;
    using Repository.Member.Models;
    using Repository.RelationshipType;

    public class RelationshipManager : IRelationshipManager
    {
        private readonly IRelationshipProvider relationshipProvider;
        private readonly IRelationshipMemberProvider relationshipMemberProvider;
        private readonly IRelationshipTypeProvider relationshipTypeProvider;

        public RelationshipManager(IRelationshipProvider relationshipProvider, IRelationshipMemberProvider relationshipMemberProvider, IRelationshipTypeProvider relationshipTypeProvider)
        {
            Contract.RequiresNotNull(relationshipProvider, nameof(relationshipProvider));
            Contract.RequiresNotNull(relationshipMemberProvider, nameof(relationshipMemberProvider));
            Contract.RequiresNotNull(relationshipTypeProvider, nameof(relationshipTypeProvider));

            this.relationshipProvider = relationshipProvider;
            this.relationshipMemberProvider = relationshipMemberProvider;
            this.relationshipTypeProvider = relationshipTypeProvider;
        }

        public async Task<Guid> CreateRelationship(
            MemberReadModel firstMember,
            MemberReadModel secondMember,
            RelationshipTypeEnum firstMemberRelationshipType,
            RelationshipTypeEnum secondMemberRelationshipType,
            DateTime? startDate,
            DateTime? endDate)
        {
            Contract.RequiresNotNull(firstMember, nameof(firstMember));
            Contract.RequiresNotNull(secondMember, nameof(secondMember));

            var firstMemberRelationshipTypeModel = await this.relationshipTypeProvider
                .GetRelationshipType(firstMemberRelationshipType).ConfigureAwait(false);

            var secondMemberRelationshipTypeModel = await this.relationshipTypeProvider
                .GetRelationshipType(secondMemberRelationshipType).ConfigureAwait(false);

            IList<RelationshipMemberModel> relationshipMembers = new List<RelationshipMemberModel>
            {
                new RelationshipMemberModel
                {
                    Member = firstMember,
                    RelationshipType = firstMemberRelationshipTypeModel
                },
                new RelationshipMemberModel
                {
                    Member = secondMember,
                    RelationshipType = secondMemberRelationshipTypeModel
                }
            };

            var relationshipId = await this.relationshipProvider.CreateRelationship(relationshipMembers, startDate, endDate)
                .ConfigureAwait(false);

            await this.relationshipMemberProvider
                .CreateRelationshipMembershipAsync(relationshipId, relationshipMembers, startDate, endDate)
                .ConfigureAwait(false);

            return relationshipId;
        }
    }
}