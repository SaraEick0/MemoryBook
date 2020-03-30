namespace MemoryBook.DataAccess.Entities.EntityBuilders
{
    using System;

    using Microsoft.EntityFrameworkCore;

    public abstract class BaseEntityBuilder : IEntityBuilder
    {
        protected BaseEntityBuilder(ModelBuilder modelBuilder)
        {
            this.ModelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));
        }

        protected ModelBuilder ModelBuilder { get; }

        public virtual void Build()
        {
        }
    }
}