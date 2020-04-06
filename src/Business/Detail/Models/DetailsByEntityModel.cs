namespace MemoryBook.Business.Detail.Models
{
    using MemoryBook.Repository.Detail.Models;
    using System;
    using System.Collections.Generic;

    public class DetailsByEntityModel
    {
        public Guid EntityId { get; set; }

        public IList<DetailReadModel> Details { get; set; }
    }
}