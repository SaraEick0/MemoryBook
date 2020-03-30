namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using Common.Extensions;
    using Microsoft.EntityFrameworkCore;

    public abstract class BaseEntityBuilder : IEntityBuilder
    {
        protected BaseEntityBuilder(ModelBuilder modelBuilder)
        {
            Contract.RequiresNotNull(modelBuilder, nameof(modelBuilder));
            this.ModelBuilder = modelBuilder;
        }

        protected ModelBuilder ModelBuilder { get; }

        public virtual void Build()
        {
        }
    }
}