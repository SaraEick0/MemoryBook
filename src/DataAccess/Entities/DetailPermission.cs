namespace MemoryBook.DataAccess.Entities
{
    using System;
    using Interfaces;

    public class DetailPermission : IHasIdProperty
    {
        public Guid Id { get; set; }

        public Member Member { get; set; }

        public Guid MemberId { get; set; }

        public Detail Detail { get; set; }

        public Guid DetailId { get; set; }

        public bool CanEdit { get; set; }

        public bool CanDelete { get; set; }
    }
}