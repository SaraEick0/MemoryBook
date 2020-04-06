namespace MemoryBook.Business.Detail.Extensions
{
    using System;
    using System.Collections.Generic;
    using Common.Extensions;
    using Models;
    using Repository.Detail.Models;
    using Repository.DetailType;
    using Repository.DetailType.Models;

    public static class DetailViewModelExtensions
    {
        public static DetailViewModel ToViewModel(this DetailReadModel readModel, IDictionary<Guid, DetailTypeReadModel> detailTypeDictionary)
        {
            Contract.RequiresNotNull(readModel, nameof(readModel));

            var detailType = detailTypeDictionary[readModel.DetailTypeId];
            Enum.TryParse<DetailTypeEnum>(detailType.Code, out var detailTypeEnum);

            return new DetailViewModel
            {
                StartDate = readModel.StartTime,
                EndDate = readModel.EndTime,
                DetailType = detailTypeEnum,
                DetailTypeStartText = detailType.DetailStartText,
                DetailTypeEndText = detailType.DetailEndText,
                DetailTypeText = readModel.CustomDetailText,
                Story = readModel.Story
            };
        }
    }
}