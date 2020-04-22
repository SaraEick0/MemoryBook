namespace MemoryBook.Business.Relationship.Models
{
    using System;
    using System.Collections.Generic;
    using Detail.Models;

    public class CombinedRelationshipReadModel : CombinedRelationshipModelBase<CombinedRelationshipMemberReadModel>
    {
        public Guid Id { get; set; }

        public IList<DetailViewModel> Details { get; set; }
    }
}