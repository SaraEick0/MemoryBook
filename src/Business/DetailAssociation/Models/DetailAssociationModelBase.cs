namespace MemoryBook.Business.DetailAssociation.Models
{
    using System;

    public abstract class DetailAssociationModelBase
    {
        public Guid EntityTypeId { get; set; }

        public Guid EntityId { get; set; }

        public Guid DetailId { get; set; }
    }
}