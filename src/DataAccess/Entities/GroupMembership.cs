namespace MemoryBook.DataAccess.Entities
{
    using System;
    using Interfaces;

    public class GroupMembership : IHasIdProperty
    {
        public Guid Id { get; set; }

        public Member Member { get; set; }

        public Guid MemberId { get; set; }

        public Group Group { get; set; }

        public Guid GroupId { get; set; }
    }
}