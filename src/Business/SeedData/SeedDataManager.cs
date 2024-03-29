﻿namespace MemoryBook.Business.SeedData
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.Extensions;
    using Microsoft.Extensions.Logging;
    using Repository.DetailType.Managers;
    using Repository.EntityType.Managers;
    using Repository.RelationshipType.Managers;
    using Repository.SeedData;

    public class SeedDataManager : ISeedDataManager
    {
        private readonly IRelationshipTypeQueryManager relationshipTypeQueryManager;
        private readonly IDetailTypeQueryManager detailTypeQueryManager;
        private readonly IEntityTypeQueryManager entityTypeQueryManager;
        private readonly IRelationshipTypeCommandManager relationshipTypeCommandManager;
        private readonly IDetailTypeCommandManager detailTypeCommandManager;
        private readonly IEntityTypeCommandManager entityTypeCommandManager;
        private readonly ILogger<SeedDataManager> logger;

        public SeedDataManager(
            IRelationshipTypeQueryManager relationshipTypeQueryManager,
            IDetailTypeQueryManager detailTypeQueryManager,
            IEntityTypeQueryManager entityTypeQueryManager,
            IRelationshipTypeCommandManager relationshipTypeCommandManager,
            IDetailTypeCommandManager detailTypeCommandManager,
            IEntityTypeCommandManager entityTypeCommandManager,
            ILogger<SeedDataManager> logger)
        {
            Contract.RequiresNotNull(relationshipTypeQueryManager, nameof(relationshipTypeQueryManager));
            Contract.RequiresNotNull(detailTypeQueryManager, nameof(detailTypeQueryManager));
            Contract.RequiresNotNull(entityTypeQueryManager, nameof(entityTypeQueryManager));
            Contract.RequiresNotNull(relationshipTypeCommandManager, nameof(relationshipTypeCommandManager));
            Contract.RequiresNotNull(detailTypeCommandManager, nameof(detailTypeCommandManager));
            Contract.RequiresNotNull(entityTypeCommandManager, nameof(entityTypeCommandManager));
            Contract.RequiresNotNull(logger, nameof(logger));

            this.relationshipTypeQueryManager = relationshipTypeQueryManager;
            this.detailTypeQueryManager = detailTypeQueryManager;
            this.entityTypeQueryManager = entityTypeQueryManager;
            this.relationshipTypeCommandManager = relationshipTypeCommandManager;
            this.detailTypeCommandManager = detailTypeCommandManager;
            this.entityTypeCommandManager = entityTypeCommandManager;
            this.logger = logger;
        }

        public async Task LoadSeedData()
        {
            try
            {
                await this.LoadEntityTypes().ConfigureAwait(false);
                await this.LoadDetailTypes().ConfigureAwait(false);
                await this.LoadRelationshipTypes().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An exception occurred in {nameof(SeedDataManager)}");
            }
        }

        private async Task LoadEntityTypes()
        {
            var entityTypes = EntityTypeSeedData.EntityTypes;
            var existing = await this.entityTypeQueryManager.GetAllEntityTypes();

            var entityTypesToCreate = entityTypes.Where(x => !existing.Any(r => r.Code.Equals(x.Code))).ToArray();
            await this.entityTypeCommandManager.CreateEntityType(entityTypesToCreate).ConfigureAwait(false);
        }

        private async Task LoadDetailTypes()
        {
            var detailTypes = DetailTypeSeedData.DetailTypes;
            var existing = await this.detailTypeQueryManager.GetAllDetailTypes();

            var detailTypesToCreate = detailTypes.Where(x => !existing.Any(r => r.Code.Equals(x.Code))).ToArray();
            await this.detailTypeCommandManager.CreateDetailType(detailTypesToCreate).ConfigureAwait(false);
        }

        private async Task LoadRelationshipTypes()
        {
            var relationshipTypes = RelationshipTypeSeedData.RelationshipTypes;
            var existing = await this.relationshipTypeQueryManager.GetAllRelationshipTypes();

            var relationshipTypesToCreate = relationshipTypes.Where(x => !existing.Any(r => r.Code.Equals(x.Code))).ToArray();
            await this.relationshipTypeCommandManager.CreateRelationshipType(relationshipTypesToCreate).ConfigureAwait(false);
        }
    }
}
