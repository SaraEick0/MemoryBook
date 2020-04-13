namespace MemoryBook.Business.Relationship.Models
{
    using System;
    using System.Collections.Generic;

    public abstract class CombinedRelationshipModelBase<T>
    where T : CombinedRelationshipMemberCreateModel
    {
        public IList<T> RelationshipMembers { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}