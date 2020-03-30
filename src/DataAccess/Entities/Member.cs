namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class Member : IHasIdProperty
	{
        public Guid Id { get; set; }

        public Guid MemoryBookUniverseId { get; set; }

        public MemoryBookUniverse MemoryBookUniverse { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "FirstName is limited to 1000 characters.")]
        public string FirstName { get; set; }                        // Members are named

        [StringLength(1000, ErrorMessage = "MiddleName is limited to 1000 characters.")]
        public string MiddleName { get; set; }                        // Members are named

        [StringLength(1000, ErrorMessage = "LastName is limited to 1000 characters.")]
        public string LastName { get; set; }                        // Members are named

        public string CommonName { get; set; }                  // Nickname

        public List<RelationshipMembership> RelationshipMemberships { get; set; }   // Relationships with other Members

        public List<Detail> CreatedDetails { get; set; }

        /// <summary>
        /// Details this member is a member of.
        /// </summary>
        public IList<DetailAssociation> DetailAssociations { get; set; }

        /// <summary>
        /// Details this member has permissions to edit or update.
        /// </summary>
        public List<DetailPermission> Permissions { get; set; }

        public List<GroupMembership> GroupMemberships { get; set; }
	}
}