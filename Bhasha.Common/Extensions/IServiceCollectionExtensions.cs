using Bhasha.Common.Arguments;
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
                .AddTransient<ChapterDtoImporter>()
                .AddTransient<IChaptersLookup, ChaptersLookup>()
                .AddTransient<IArgumentAssemblyProvider, ArgumentAssemblyProvider>()
                .AddTransient<IAssembleChapters, ChapterAssembly>()
                .AddTransient<ICheckResult, ResultChecker>()
                .AddTransient<IUpdateStatsForEvaluation, EvaluationStatsUpdater>()
                .AddTransient<IUpdateStatsForTip, TipStatsUpdater>()
                .AddTransient<IEvaluateSubmit, SubmitEvaluator>()
                .AddTransient<OneOutOfFourArgumentsAssembly>();
        }
    }
}
