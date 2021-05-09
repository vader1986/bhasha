using System;
using System.Collections.Generic;
using Bhasha.Common.Arguments;
using Bhasha.Common.Database;
using Bhasha.Common.Importers;
using Bhasha.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Bhasha.Common.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddBhashaServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IConvert<DbTranslatedWord, TranslatedWord>, DatabaseTypeConverter>()
                .AddTransient<IConvert<DbTranslatedExpression, TranslatedExpression>, DatabaseTypeConverter>()
                .AddTransient<IConvert<DbTranslatedChapter, Chapter>, DatabaseTypeConverter>()
                .AddTransient<IConvert<DbUserProfile, Profile>, DatabaseTypeConverter>()
                .AddTransient<IConvert<DbStats, Stats>, DatabaseTypeConverter>()
                .AddTransient<IConvert<IEnumerable<string>, string>, WordsPhraseConverter>()
                .AddTransient<IProvideTips, ProvideTips>()
                .AddTransient<ChapterImporter>()
                .AddTransient<IArgumentAssemblyProvider, ArgumentAssemblyProvider>()
                .AddTransient<IChaptersLookup, ChaptersLookup>()
                .AddTransient<ICheckResult, ResultChecker>()
                .AddTransient<IEvaluateSubmit, EvaluateSubmit>()
                .AddTransient<ITranslate<DbChapter, Chapter, Profile>, TranslateChapter>()
                .AddTransient<ITranslate<Guid, TranslatedExpression>, TranslateExpression>()
                .AddTransient<ITranslate<Guid, TranslatedWord>, TranslateWord>()
                .AddTransient<IUpdateStatsOnSubmit, UpdateStatsOnSubmit>()
                .AddTransient<IUpdateStatsOnTipRequest, UpdateStatsOnTipRequest>()
                .AddTransient<OneOutOfFourArgumentsAssembly>();
        }
    }
}
