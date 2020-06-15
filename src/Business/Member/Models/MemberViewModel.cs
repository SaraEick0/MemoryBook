namespace MemoryBook.Business.Member.Models
{
    using System;
    using System.Collections.Generic;
    using Detail.Models;
    using Relationship.Models;

    public class MemberViewModel
    {
        public Guid Id { get; set; }

        public Guid MemoryBookUniverseId { get; set; }

        public string FullName { get; set; }

        public string CommonName { get; set; }

        public IList<DetailViewModel> Details { get; set; }

        public IList<CombinedRelationshipReadModel> Relationships { get; set; }
    }
}