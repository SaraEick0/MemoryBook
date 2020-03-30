using System;

namespace MemoryBook.DataAccess.Entities
{
    using Interfaces;

    public class DetailAssociation : IHasIdProperty
    {
        public Guid Id { get; set; }

        public Guid EntityId { get; set; }

        public EntityType EntityType { get; set; }

        public Guid EntityTypeId { get; set; }

        public Detail Detail { get; set; }

        public Guid DetailId { get; set; }
    }
}