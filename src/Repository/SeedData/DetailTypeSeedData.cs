namespace MemoryBook.Repository.SeedData
{
    using System.Collections.Generic;
    using DetailType;
    using DetailType.Models;

    public static class DetailTypeSeedData
    {
        public static IList<DetailTypeCreateModel> DetailTypes => new List<DetailTypeCreateModel>
        {
            new DetailTypeCreateModel { Code = DetailTypeEnum.LifeSpan.ToString(), DetailStartText = "Born", DetailEndText = "Died" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.Divorce.ToString(), DetailStartText = "Divorced", DetailEndText = "Divorced" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.EndJob.ToString(), DetailStartText = "Left Job", DetailEndText = "Left Job" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.StartJob.ToString(), DetailStartText = "Started Job", DetailEndText = "Started Job" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.Wedding.ToString(), DetailStartText = "Got Married", DetailEndText = "Got Married" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.Graduated.ToString(), DetailStartText = "Graduated", DetailEndText = "Graduated" },
            new DetailTypeCreateModel { Code = DetailTypeEnum.Event.ToString(), DetailStartText = "Started", DetailEndText = "Ended" },
        };
    }
}