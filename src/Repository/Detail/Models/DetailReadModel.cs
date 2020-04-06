namespace MemoryBook.Repository.Detail.Models
{
    using System;
    using System.Collections.Generic;

    public class DetailReadModel : DetailModelBase
    {
        public Guid Id { get; set; }

        public IList<Guid> EntityIds { get; set; }

        public string DetailTypeCode { get; set; }

        public IList<Guid> MemberIds { get; set; } = new List<Guid>();

        public IList<Guid> EditorIds { get; set; } = new List<Guid>();
    }
}