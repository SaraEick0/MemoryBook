namespace MemoryBook.DataAccess.Extensions
{
    using System;
    using System.Linq.Expressions;
    using Interfaces;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> Has<TEntity>(
          this EntityTypeBuilder<TEntity> typeBuilder,
          Action<EntityTypeBuilder<TEntity>> builderAction)
          where TEntity : class
        {
            builderAction(typeBuilder);
            return typeBuilder;
        }

        public static EntityTypeBuilder<TEntity> HasUniqueCode<TEntity>(
          this EntityTypeBuilder<TEntity> typeBuilder,
          int length = 50)
          where TEntity : class, IHasCodeProperty
        {
            typeBuilder.Property<string>(e => e.Code).HasMaxLength(length);
            typeBuilder.HasIndex(e => e.Code).IsUnique();
            return typeBuilder;
        }

        public static EntityTypeBuilder<TEntity> HasUniqueName<TEntity>(
            this EntityTypeBuilder<TEntity> typeBuilder,
            int length = 1000)
            where TEntity : class, IHasNameProperty
        {
            typeBuilder.Property<string>(e => e.Name).HasMaxLength(length);
            typeBuilder.HasIndex(e => e.Name).IsUnique();
            return typeBuilder;
        }

        public static string GetPropertyName<TEntity, TProperty>(
          this EntityTypeBuilder<TEntity> ignored,
          Expression<Func<TEntity, TProperty>> propertyGetter)
          where TEntity : class
        {
            return EntityTypeBuilderExtensions.GetPropertyName(propertyGetter?.Body);
        }

        private static string GetPropertyName(Expression expression)
        {
            if (!(expression is MemberExpression memberExpression))
                return (string)null;
            string propertyName = EntityTypeBuilderExtensions.GetPropertyName(memberExpression.Expression);
            return propertyName != null ? propertyName + "." + memberExpression.Member.Name : memberExpression.Member.Name;
        }
    }
}