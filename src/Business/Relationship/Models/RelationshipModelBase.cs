namespace MemoryBook.Business.Relationship.Models
{
    using System;

    public abstract class RelationshipModelBase
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}