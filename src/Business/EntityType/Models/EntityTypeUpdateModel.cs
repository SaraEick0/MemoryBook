namespace MemoryBook.Business.EntityType.Models
{
    using System;

    public class EntityTypeUpdateModel : EntityTypeCreateModel
    {
        public Guid Id { get; set; }
    }
}