using System;

namespace Bhasha.Common.Tests.Support
{
    public class EntityFactory
    {
        public static TProduct Build<TProduct>(Guid? id = default) where TProduct : class, IEntity
        {
            if (typeof(TProduct) == typeof(ChapterStats))
            {
                return ChapterStatsBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            if (typeof(TProduct) == typeof(GenericChapter))
            {
                return GenericChapterBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;

            }

            if (typeof(TProduct) == typeof(Profile))
            {
                return ProfileBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            if (typeof(TProduct) == typeof(Tip))
            {
                return TipBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            if (typeof(TProduct) == typeof(Token))
            {
                return TokenBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            if (typeof(TProduct) == typeof(Translation))
            {
                return TranslationBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            if (typeof(TProduct) == typeof(User))
            {
                return UserBuilder
                    .Default
                    .WithId(id ?? Guid.NewGuid())
                    .Build() as TProduct;
            }

            throw new InvalidOperationException(
                $"found no builder for {typeof(TProduct).Name}");
        }
    }
}
