namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Interfaces;
    
    public class EntityType : IHasIdProperty, IHasCodeProperty
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code is limited to 50 characters.")]
        public string Code { get; set; }

        public IList<DetailAssociation> DetailAssociations { get; set; }
    }
}