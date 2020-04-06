namespace MemoryBook.Business.Member.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Providers;
    using Repository.Member.Models;

    public class MemberManager : IMemberManager
    {
        private readonly IMemberProvider memberProvider;

        public MemberManager(IMemberProvider memberProvider)
        {
            Contract.RequiresNotNull(memberProvider, nameof(memberProvider));

            this.memberProvider = memberProvider;
        }

        public async Task<MemberReadModel> CreateMember(Guid memoryBookUniverseId, string firstName, string middleName, string lastName, string commonName)
        {
            Contract.RequiresNotNullOrWhitespace(firstName, nameof(firstName));
            Contract.RequiresNotNullOrWhitespace(lastName, nameof(lastName));
            Contract.RequiresNotNullOrWhitespace(commonName, nameof(commonName));

            IList<MemberReadModel> allMembers = await this.memberProvider.GetAllMembers(memoryBookUniverseId).ConfigureAwait(false);

            if (allMembers.Any(x => x.CommonName.Equals(commonName, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Member already existed with common name {commonName} for universe {memoryBookUniverseId}");
            }

            return await this.memberProvider.CreateMember(memoryBookUniverseId, firstName, middleName, lastName, commonName)
                .ConfigureAwait(false);
        }
    }
}