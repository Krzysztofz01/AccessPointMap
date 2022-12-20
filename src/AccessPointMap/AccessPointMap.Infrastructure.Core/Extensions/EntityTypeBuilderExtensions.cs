using AccessPointMap.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace AccessPointMap.Infrastructure.Core.Extensions
{
#nullable enable
    public static class EntityTypeBuilderExtensions
    {
        public static KeyBuilder HasPublicKey<TAggregateRoot>(this EntityTypeBuilder<TAggregateRoot> entityTypeBuilder)
            where TAggregateRoot : AggregateRoot
        {
            var keyBuilder = entityTypeBuilder.HasKey(e => e.Id);
            entityTypeBuilder.Property(e => e.Id).ValueGeneratedNever();

            return keyBuilder;
        }

        public static void UseSoftDelete<TAggragateRoot>(this EntityTypeBuilder<TAggragateRoot> entityTypeBuilder)
            where TAggragateRoot : AggregateRoot
        {
            entityTypeBuilder.Property(e => e.DeletedAt).HasDefaultValue(null);
            entityTypeBuilder.HasQueryFilter(e => e.DeletedAt == null);
        }

        public static OwnedNavigationBuilder<TEntity, TValueObject> OwnsOneValueObject<TEntity, TValueObject>(this EntityTypeBuilder<TEntity> entityTypeBuilder, Expression<Func<TEntity, TValueObject?>> navigationExpression)
            where TEntity : Entity
            where TValueObject : ValueObject<TValueObject>
        {
            var navigationBuilder = entityTypeBuilder.OwnsOne(navigationExpression);
            entityTypeBuilder.Navigation(navigationExpression).IsRequired();

            return navigationBuilder;
        }

        // TODO: Implement a generic constraint to ensure that the value object is in deed a shared value object
        public static OwnedNavigationBuilder<TEntity, TValueObject> OwnsOneSharedValueObject<TEntity, TValueObject>(this EntityTypeBuilder<TEntity> entityTypeBuilder, Expression<Func<TEntity, TValueObject?>> navigationExpression)
            where TEntity : Entity
            where TValueObject : class
        {
            var navigationBuilder = entityTypeBuilder.OwnsOne(navigationExpression);
            entityTypeBuilder.Navigation(navigationExpression).IsRequired();

            return navigationBuilder;
        }
    }
#nullable disable
}
