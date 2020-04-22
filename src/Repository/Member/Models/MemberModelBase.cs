namespace MemoryBook.Repository.Member.Models
{
    using System;
    using System.Collections.Generic;

    public abstract class MemberModelBase
	{
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string CommonName { get; set; }                  // Nickname

		public IList<Guid> GroupIds { get; set; } = new List<Guid>();

		public IList<Guid> RelationshipIds { get; set; } = new List<Guid>();

		public IList<Guid> DetailIds { get; set; } = new List<Guid>();
    }
}