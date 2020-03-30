namespace MemoryBook.Repository.Member.Managers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Business.Member.Managers;
    using Business.Member.Models;

    public class MemberManager : IMemberManager
    {
        private readonly IMemberCommandManager memberCommandManager;
        private readonly IMemberQueryManager memberQueryManager;

        public MemberManager(IMemberCommandManager memberCommandManager, IMemberQueryManager memberQueryManager)
        {
            this.memberCommandManager = memberCommandManager ?? throw new ArgumentNullException(nameof(memberCommandManager));
            this.memberQueryManager = memberQueryManager ?? throw new ArgumentNullException(nameof(memberQueryManager));
        }

        public async Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                throw new ArgumentException(nameof(firstName));
            }
            if (string.IsNullOrWhiteSpace(lastName))
            {
                throw new ArgumentException(nameof(lastName));
            }
            if (string.IsNullOrWhiteSpace(commonName))
            {
                throw new ArgumentException(nameof(commonName));
            }

            var allMembers = await this.memberQueryManager.GetAllMembers(memoryBookUniverseId).ConfigureAwait(false);

            if (allMembers.Any(x => x.CommonName.Equals(x.CommonName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Member already existed for with common name {commonName} for universe {memoryBookUniverseId}");
            }

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