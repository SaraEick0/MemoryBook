namespace MemoryBook.DataAccess.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Interfaces;

    public class DetailType : IHasIdProperty, IHasCodeProperty
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Code is limited to 50 characters.")]
        public string Code { get; set; }

        public string DetailStartText { get; set; }

        public string DetailEndText { get; set; }

        public IList<Detail> Details { get; set; }
    }
}