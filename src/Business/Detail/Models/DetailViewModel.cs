namespace MemoryBook.Business.Detail.Models
{
    using System;
    using Repository.DetailType;

    public class DetailViewModel
    {
        public string DetailTypeText { get; set; }

        public DetailTypeEnum DetailType { get; set; }

        public string DetailTypeStartText { get; set; }

        public string DetailTypeEndText { get; set; }

        public string Story { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}