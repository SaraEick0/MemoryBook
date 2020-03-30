namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Extensions;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class BaseSqlServerEntityBuilder<TEntity> : BaseEntityBuilder
        where TEntity : class, IHasIdProperty
    {
        public BaseSqlServerEntityBuilder(ModelBuilder modelBuilder)
            : base(modelBuilder)
        {
        }

        public override void Build()
        {
            this.Configure(this.ModelBuilder.GetSqlServerBuilderFor<TEntity>());
        }

        protected virtual void Configure(EntityTypeBuilder<TEntity> typeBuilder)
        {
        }
    }
}