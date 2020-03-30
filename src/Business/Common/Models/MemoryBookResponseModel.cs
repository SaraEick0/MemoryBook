namespace MemoryBook.Business.Common.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    public class MemoryBookResponseModel
    {
        public bool Success => !this.Errors.Any();

        public IList<string> Errors { get; set; } = new List<string>();

        public IList<Guid> Ids { get; set; }
    }
}