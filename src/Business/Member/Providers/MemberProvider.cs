namespace MemoryBook.Business.Member.Providers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Repository.Member.Managers;
    using Repository.Member.Models;

    public class MemberProvider : IMemberProvider
    {
        private readonly IMemberCommandManager memberCommandManager;
        private readonly IMemberQueryManager memberQueryManager;

        public MemberProvider(IMemberCommandManager memberCommandManager, IMemberQueryManager memberQueryManager)
        {
            Contract.RequiresNotNull(memberCommandManager, nameof(memberCommandManager));
            Contract.RequiresNotNull(memberQueryManager, nameof(memberQueryManager));

            this.memberCommandManager = memberCommandManager ?? throw new ArgumentNullException(nameof(memberCommandManager));
            this.memberQueryManager = memberQueryManager ?? throw new ArgumentNullException(nameof(memberQueryManager));
        }

        public async Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName)
        {
            Contract.RequiresNotNullOrWhitespace(firstName, nameof(firstName));
            Contract.RequiresNotNullOrWhitespace(lastName, nameof(lastName));
            Contract.RequiresNotNullOrWhitespace(commonName, nameof(commonName));

            MemberCreateModel memberCreateModel = new MemberCreateModel
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                CommonName = commonName
            };

            var id = await this.memberCommandManager.CreateMembers(memoryBookUniverseId, memberCreateModel)
                .ConfigureAwait(false);

            if (id == null || id.Count == 0)
            {
                return null;
            }

            var memberReadModel = await this.memberQueryManager.GetMembers(memoryBookUniverseId, id)
                .ConfigureAwait(false);

            return memberReadModel.FirstOrDefault();
        }
    }
}