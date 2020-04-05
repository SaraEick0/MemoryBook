namespace MemoryBook.Repository.Member.Models
{
    using System.Collections.Generic;
    using Detail.Models;
    using Group.Models;
    using Relationship.Models;

    public abstract class MemberModelBase
	{
		public string FirstName { get; set; }

		public string MiddleName { get; set; }

		public string LastName { get; set; }

		public string CommonName { get; set; }                  // Nickname

		public IList<GroupReadModel> Groups { get; set; } = new List<GroupReadModel>();

		public IList<RelationshipReadModel> Relationships { get; set; } = new List<RelationshipReadModel>();

		public IList<DetailReadModel> Details { get; set; } = new List<DetailReadModel>();
    }
}