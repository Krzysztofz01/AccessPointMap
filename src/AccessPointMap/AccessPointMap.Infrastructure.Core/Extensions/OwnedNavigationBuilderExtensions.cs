using AccessPointMap.Domain.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace AccessPointMap.Infrastructure.Core.Extensions
{
#nullable enable
    public static class OwnedNavigationBuilderExtensions
    {
        public static KeyBuilder HasPublicKey<TAggregateRoot, TEntity>(this OwnedNavigationBuilder<TAggregateRoot, TEntity> ownedNavigationBuilder)
            where TAggregateRoot : AggregateRoot
            where TEntity : Entity
        {
            var aggregateRootName = typeof(TAggregateRoot).Name.ToLower();
            ownedNavigationBuilder.WithOwner().HasForeignKey($"{aggregateRootName}Id");

            var keyBuilder = ownedNavigationBuilder.HasKey(e => e.Id);
            ownedNavigationBuilder.Property(e => e.Id).ValueGeneratedNever();

            return keyBuilder;
        }

        public static void UseSoftDelete<TParentEntity, TChildEntity>(this OwnedNavigationBuilder<TParentEntity, TChildEntity> ownedNavigationBuilder)
            where TParentEntity : Entity
            where TChildEntity : Entity
        {
            ownedNavigationBuilder.Property(e => e.DeletedAt).HasDefaultValue(null);
        }

        public static OwnedNavigationBuilder<TChildEntity, TValueObject> OwnsOneValueObject<TParentEntity, TChildEntity, TValueObject>(this OwnedNavigationBuilder<TParentEntity, TChildEntity> entityTypeBuilder, Expression<Func<TChildEntity, TValueObject?>> navigationExpression)
            where TParentEntity : AggregateRoot
            where TChildEntity : Entity
            where TValueObject : ValueObject<TValueObject>
        {
            var navigationBuilder = entityTypeBuilder.OwnsOne(navigationExpression);
            entityTypeBuilder.Navigation(navigationExpression).IsRequired();

            return navigationBuilder;
        }

        // TODO: Implement a generic constraint to ensure that the value object is in deed a shared value object
        public static OwnedNavigationBuilder<TChildEntity, TValueObject> OwnsOneSharedValueObject<TParentEntity, TChildEntity, TValueObject>(this OwnedNavigationBuilder<TParentEntity, TChildEntity> entityTypeBuilder, Expression<Func<TChildEntity, TValueObject?>> navigationExpression)
            where TParentEntity : Entity
            where TChildEntity : Entity
            where TValueObject : class
        {
            var navigationBuilder = entityTypeBuilder.OwnsOne(navigationExpression);
            entityTypeBuilder.Navigation(navigationExpression).IsRequired();

            return navigationBuilder;
        }
    }
#nullable disable
}
