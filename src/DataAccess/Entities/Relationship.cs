namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using Interfaces;

    public class Relationship : IHasIdProperty
    {
        public Guid Id { get; set; }

        public Guid MemoryBookUniverseId { get; set; }

        public MemoryBookUniverse MemoryBookUniverse { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public List<RelationshipMembership> RelationshipMemberships { get; set; }
    }
}