namespace MemoryBook.Repository.EntityType.Models
{
    using System;

    public class EntityTypeUpdateModel : EntityTypeCreateModel
    {
        public Guid Id { get; set; }
    }
}