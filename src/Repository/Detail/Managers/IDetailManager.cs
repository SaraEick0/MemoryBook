namespace MemoryBook.Repository.Detail.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Business.Detail.Models;
    using Business.Member.Models;

    public interface IDetailManager
    {
        Task<DetailReadModel> CreateBirthday(MemberReadModel creator, MemberReadModel member, DateTime birthday, string birthplace = null);

        Task<DetailReadModel> CreateWedding(MemberReadModel creator, MemberReadModel spouseOne, MemberReadModel spouseTwo, DateTime weddingDate, string weddingLocation = null);

        Task<DetailReadModel> CreateEvent(MemberReadModel creator, IList<MemberReadModel> memberAttendees, DateTime? startDate, DateTime? endDate, string description);
    }
}