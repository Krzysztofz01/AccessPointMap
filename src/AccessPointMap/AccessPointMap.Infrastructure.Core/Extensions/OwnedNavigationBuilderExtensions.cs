using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq.Expressions;

namespace AccessPointMap.Infrastructure.Core.Extensions
{
    public static class OwnedNavigationBuilderExtensions
    {
#nullable enable
        public static void OwnsRequiredOne<TOwnerEntity, TDependentEntity, TNewDependentEntity>(this OwnedNavigationBuilder<TOwnerEntity, TDependentEntity> ownedNavigationBuilder, Expression<Func<TDependentEntity, TNewDependentEntity?>> navigationExpression) where TOwnerEntity : class where TDependentEntity : class where TNewDependentEntity : class
        {
            ownedNavigationBuilder.OwnsOne(navigationExpression);
            ownedNavigationBuilder.Navigation(navigationExpression).IsRequired();
        }
#nullable disable
    }
}
