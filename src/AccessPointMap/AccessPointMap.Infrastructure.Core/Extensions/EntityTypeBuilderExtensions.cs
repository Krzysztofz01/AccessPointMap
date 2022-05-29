using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace AccessPointMap.Infrastructure.Core.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
#nullable enable
        public static void OwnsRequiredOne<TEntity, TRelatedEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, Expression<Func<TEntity, TRelatedEntity?>> navigationExpression) where TRelatedEntity : class where TEntity : class
        {
            entityTypeBuilder.OwnsOne(navigationExpression);
            entityTypeBuilder.Navigation(navigationExpression).IsRequired();
        }
#nullable disable
    }
}
