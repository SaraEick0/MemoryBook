namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class MemoryBookUniverse : IHasIdProperty, IHasNameProperty
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Name is limited to 1000 characters.")]
        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public IList<Member> Members { get; set; }

        public IList<Group> Groups { get; set; }

        public IList<Relationship> Relationships { get; set; }
    }
}