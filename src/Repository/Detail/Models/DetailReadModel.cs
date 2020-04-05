namespace MemoryBook.Repository.Detail.Models
{
    using System;
    using System.Collections.Generic;
    using DetailType;
    using DetailType.Models;
    using Member.Models;

    public class DetailReadModel : DetailModelBase
    {
        public Guid Id { get; set; }

        public DetailTypeReadModel DetailType { get; set; }

        public Guid? GroupId { get; set; }

        public Guid? RelationshipId { get; set; }

        public IList<Guid> MemberIds { get; set; } = new List<Guid>();

        public MemberReadModel Creator { get; set; }

        public List<MemberReadModel> Editors { get; set; }

        public string DetailTypeText
        {
            get
            {
                if (this.DetailType.Code == DetailTypeEnum.Custom.ToString())
                {
                    return CustomDetailText;
                }

                return this.DetailType.Code;
            }
        }
    }
}