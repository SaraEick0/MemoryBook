namespace MemoryBook.Business.Member.Models
{
    using System;
    using DataAccess.Interfaces;

    public class MemberReadModel : MemberModelBase, IHasIdProperty
	{
		public Guid Id { get; set; }

        public Guid MemoryBookUniverseId { get; set; }
    }
}