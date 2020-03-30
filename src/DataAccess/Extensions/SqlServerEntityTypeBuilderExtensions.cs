namespace MemoryBook.DataAccess.Extensions
{
    using System;
    using Common;
    using Common.Extensions;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class SqlServerEntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> GetSqlServerBuilderFor<TEntity>(
            this ModelBuilder modelBuilder)
            where TEntity : class, IHasIdProperty
        {
            Contract.RequiresNotNull(modelBuilder, nameof(modelBuilder));

            return modelBuilder.GetSqlServerBuilderFor<TEntity, Guid>();
        }

        public static EntityTypeBuilder<TEntity> GetSqlServerBuilderFor<TEntity, TId>(
            this ModelBuilder modelBuilder)
            where TId : struct, IEquatable<TId>
            where TEntity : class, IHasIdProperty
        {
            Contract.RequiresNotNull(modelBuilder, nameof(modelBuilder));

            EntityTypeBuilder<TEntity> builder = modelBuilder.Entity<TEntity>();

            builder
                .Property(e => e.Id)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();

            return builder;
        }
    }
}