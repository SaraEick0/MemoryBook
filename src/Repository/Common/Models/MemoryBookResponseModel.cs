namespace MemoryBook.Repository.Common.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MemoryBookResponseModel
    {
        public bool Success => !this.Errors.Any();

        public IList<string> Errors { get; set; } = new List<string>();

        public IList<Guid> Ids { get; set; }
    }
}