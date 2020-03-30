namespace MemoryBook.Business.MemoryBookUniverse.Models
{
    using System;

    public class MemoryBookUniverseReadModel : MemoryBookUniverseCreateModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}