namespace MemoryBook.Repository.Member.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Member.Managers;
    using Business.Member.Models;
    using Business.RelationshipType;
    using Common.Extensions;
    using Extensions;
    using Models;
    using Providers;

    public class MemberManager : IMemberManager
    {
        private readonly IMemberProvider memberProvider;
        private readonly IRelationshipProvider relationshipProvider;
        private readonly IRelationshipTypeProvider relationshipTypeProvider;
        private readonly IMemberQueryManager memberQueryManager;

        public MemberManager(IMemberProvider memberProvider, IRelationshipProvider relationshipProvider, IRelationshipTypeProvider relationshipTypeProvider, IMemberQueryManager memberQueryManager)
        {
            Contract.RequiresNotNull(memberProvider, nameof(memberProvider));
            Contract.RequiresNotNull(relationshipProvider, nameof(relationshipProvider));
            Contract.RequiresNotNull(relationshipTypeProvider, nameof(relationshipTypeProvider));
            Contract.RequiresNotNull(memberQueryManager, nameof(memberQueryManager));

            this.memberProvider = memberProvider;
            this.relationshipProvider = relationshipProvider;
            this.relationshipTypeProvider = relationshipTypeProvider;
            this.memberQueryManager = memberQueryManager;
        }

        public async Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName)
        {
            Contract.RequiresNotNullOrWhitespace(firstName, nameof(firstName));
            Contract.RequiresNotNullOrWhitespace(lastName, nameof(lastName));
            Contract.RequiresNotNullOrWhitespace(commonName, nameof(commonName));

            IList<MemberReadModel> allMembers = await this.memberQueryManager.GetAllMembers(memoryBookUniverseId).ConfigureAwait(false);

            if (allMembers.Any(x => x.CommonName.Equals(commonName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Member already existed with common name {commonName} for universe {memoryBookUniverseId}");
            }

            return await this.memberProvider.CreateMember(memoryBookUniverseId, firstName, middleName, lastName, commonName)
                .ConfigureAwait(false);
        }

        public async Task CreateRelationship(
            Guid memoryBookUniverseId,
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

            var relationships = await this.relationshipProvider
                .GetRelationships(memoryBookUniverseId, new List<Guid> {relationshipId})
                .ConfigureAwait(false);

            var relationship = relationships.FirstOrDefault();

            firstMember.AddRelationship(relationship);
            secondMember.AddRelationship(relationship);
        }
    }
}