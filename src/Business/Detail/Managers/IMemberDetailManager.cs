namespace MemoryBook.Business.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Repository.Detail.Models;
    using Repository.Member.Models;

    public interface IMemberDetailManager
    {
        Task<DetailReadModel> CreateBirthday(MemberReadModel creator, MemberReadModel member, DateTime birthday, string birthplace = null);

        Task<DetailReadModel> CreateEvent(MemberReadModel creator, IList<MemberReadModel> memberAttendees, DateTime? startDate, DateTime? endDate, string description);
    }
}