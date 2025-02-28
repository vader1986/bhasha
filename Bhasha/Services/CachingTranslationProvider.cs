using Bhasha.Domain;
using Bhasha.Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Bhasha.Services;

public sealed class CachingTranslationProvider(IServiceProvider serviceProvider) : ITranslationProvider
{
    private readonly MemoryCache _cache = new(new MemoryCacheOptions
    {
        ExpirationScanFrequency = TimeSpan.FromHours(4)
    });

    public async Task<Translation?> Find(int expressionId, string language, CancellationToken token = default)
    {
        if (_cache.TryGetValue((expressionId, language), out var cacheEntry))
        {
            return cacheEntry as Translation;
        }
        
        var scope = serviceProvider.CreateScope();
        var repository = scope.ServiceProvider.GetRequiredService<ITranslationRepository>();

        var translation = await repository.Find(expressionId, language, token);

        _cache.Set((expressionId, language), translation, DateTimeOffset.UtcNow.AddDays(1));

        return translation;
    }
}