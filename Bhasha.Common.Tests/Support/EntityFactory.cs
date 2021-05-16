using System;
using Bhasha.Common.Database;

namespace Bhasha.Common.Tests.Support
{
    public class EntityFactory
    {
        public static TProduct Build<TProduct>(Guid? id = default) where TProduct : class, IEntity
        {
            if (typeof(TProduct) == typeof(DbStats))
            {
                return (DbStatsBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            if (typeof(TProduct) == typeof(DbChapter))
            {
                return (DbChapterBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            if (typeof(TProduct) == typeof(DbTranslatedChapter))
            {
                return (DbTranslatedChapterBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            if (typeof(TProduct) == typeof(DbUserProfile))
            {
                return (DbUserProfileBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            if (typeof(TProduct) == typeof(DbExpression))
            {
                return (DbExpressionBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            if (typeof(TProduct) == typeof(DbWord))
            {
                return (DbWordBuilder.Default.WithId(id ?? Guid.NewGuid()).Build() as TProduct)!;
            }

            throw new InvalidOperationException($"found no builder for {typeof(TProduct).Name}");
        }
    }
}
