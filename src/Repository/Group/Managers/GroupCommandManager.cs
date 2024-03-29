﻿namespace MemoryBook.Repository.Group.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Models;
    using DataAccess;
    using DataAccess.Entities;
    using MemoryBook.Common.Extensions;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GroupCommandManager : IGroupCommandManager
    {
        private readonly MemoryBookDbContext databaseContext;

        public GroupCommandManager(MemoryBookDbContext databaseContext)
        {
            Contract.RequiresNotNull(databaseContext, nameof(databaseContext));
            this.databaseContext = databaseContext;
        }

        public async Task<MemoryBookResponseModel> CreateGroups(Guid memoryBookUniverseId, params GroupCreateModel[] models)
        {
            MemoryBookResponseModel response = new MemoryBookResponseModel();

            if (models == null || !models.Any())
            {
                return response;
            }

            IList<Group> entities = new List<Group>();

            foreach (var model in models)
            {
                Group entity = CreateEntity(memoryBookUniverseId, model);
                entities.Add(entity);
            }

            this.databaseContext.AddRange(entities);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);

            response.Ids = entities.Select(x => x.Id).ToList();

            return response;
        }

        public async Task UpdateGroups(Guid memoryBookUniverseId, params GroupUpdateModel[] models)
        {
            if (models == null || !models.Any())
            {
                return;
            }

            var groupDictionary = databaseContext.Set<Group>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Group> entitiesToUpdate = new List<Group>();
            foreach (var model in models)
            {
                if (model == null || !groupDictionary.TryGetValue(model.Id, out var entity))
                {
                    continue;
                }

                entity.Name = model.Name;
                entity.Code = model.Code;
                entity.Description = model.Description;

                entitiesToUpdate.Add(entity);
            }

            this.databaseContext.AddRange(entitiesToUpdate);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteGroups(Guid memoryBookUniverseId, params Guid[] groupIds)
        {
            if (groupIds == null || !groupIds.Any())
            {
                return;
            }

            var groupDictionary = databaseContext.Set<Group>()
                .Where(x => x.MemoryBookUniverseId == memoryBookUniverseId)
                .AsNoTracking()
                .ToDictionary(x => x.Id);

            IList<Group> entitiesToDelete = new List<Group>();
            foreach (var id in groupIds)
            {
                if (!groupDictionary.TryGetValue(id, out var entity))
                {
                    continue;
                }

                entitiesToDelete.Add(entity);
            }

            this.databaseContext.RemoveRange(entitiesToDelete);

            await this.databaseContext.SaveChangesAsync().ConfigureAwait(false);
        }

        private static Group CreateEntity(Guid memoryBookUniverseId, GroupCreateModel model)
        {
            return new Group
            {
                MemoryBookUniverseId = memoryBookUniverseId,
                Code = model.Code,
                Name = model.Name,
                Description = model.Description
            };
        }
    }
}