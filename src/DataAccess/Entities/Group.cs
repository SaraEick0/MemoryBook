namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class Group : IHasIdProperty, IHasCodeProperty, IHasNameProperty
    {
        public Guid Id { get; set; }

        public Guid MemoryBookUniverseId { get; set; }

        public MemoryBookUniverse MemoryBookUniverse { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Name is limited to 1000 characters.")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code is limited to 50 characters.")]
        public string Code { get; set; }

        public string Description { get; set; }

        public IList<GroupMembership> GroupMemberships { get; set; }
    }
}