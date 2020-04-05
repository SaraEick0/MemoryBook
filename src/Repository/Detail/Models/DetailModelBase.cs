namespace MemoryBook.Repository.Detail.Models
{
    using System;

    public abstract class DetailModelBase
    {
        public Guid DetailTypeId { get; set; }

        public string CustomDetailText { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public string Story { get; set; }
    }
}