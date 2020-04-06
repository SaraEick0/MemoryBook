namespace MemoryBook.Business.Relationship.Models
{
    using System;
    using System.Collections.Generic;
    using Detail.Models;
    using Repository.RelationshipType;

    public class RelationshipViewModel
    {
        public string FirstMemberName { get; set; }

        public Guid FirstMemberId { get; set; }

        public RelationshipTypeEnum FirstMemberRelationshipType { get; set; }

        public string SecondMemberName { get; set; }

        public Guid SecondMemberId { get; set; }

        public RelationshipTypeEnum SecondMemberRelationshipType { get; set; }

        public IList<DetailViewModel> Details { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}