namespace MemoryBook.Business.GroupMembership.Models
{
    using System;

    public class GroupMembershipCreateModel
    {
        public Guid GroupId { get; set; }

        public Guid MemberId { get; set; }
    }
}