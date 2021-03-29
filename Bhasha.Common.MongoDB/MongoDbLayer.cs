using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Services;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public class MongoDbLayer : IDatabase
    {
        private readonly IMongoDb _db;
        private readonly Converter _converter;

        public MongoDbLayer(IMongoDb db, Converter converter)
        {
            _db = db;
            _converter = converter;
        }

        public async Task<Token?> QueryTokenByLabel(string label)
        {
            var token = await _db
                .GetCollection<TokenDto>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.Label == label);

            return token != null ? _converter.Convert(token) : default;
        }

        public async Task<IEnumerable<GenericChapter>> QueryChaptersByLevel(int level)
        {
            var chapters = await _db
                .GetCollection<GenericChapterDto>()
                .AsQueryable()
                .Where(x => x.Level == level)
                .ToListAsync();

            return chapters.Select(_converter.Convert);
        }

        public async Task<IEnumerable<Profile>> QueryProfilesByUserId(string userId)
        {
            var profiles = await _db
                .GetCollection<ProfileDto>()
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return profiles.Select(_converter.Convert);
        }

        public async Task<ChapterStats?> QueryStatsByChapterAndProfileId(Guid chapterId, Guid profileId)
        {
            var stats = await _db
                .GetCollection<ChapterStatsDto>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.ChapterId == chapterId &&
                                          x.ProfileId == profileId);

            return stats == default ? default : _converter.Convert(stats);
        }

        public async Task<IEnumerable<ChapterStats>> QueryStatsByProfileId(Guid profileId)
        {
            var stats = await _db
                .GetCollection<ChapterStatsDto>()
                .AsQueryable()
                .Where(x => x.ProfileId == profileId)
                .ToListAsync();

            return stats.Select(_converter.Convert);
        }

        public async Task<IEnumerable<Tip>> QueryTips(Guid chapterId, int pageIndex)
        {
            var tips = await _db
                .GetCollection<TipDto>()
                .AsQueryable()
                .Where(x => x.ChapterId == chapterId &&
                            x.PageIndex == pageIndex)
                .ToListAsync();

            return tips.Select(_converter.Convert);
        }

        public async Task<Translation?> QueryTranslationByTokenId(Guid tokenId, Language language)
        {
            var translation = await _db
                .GetCollection<TranslationDto>()
                .AsQueryable()
                .FirstOrDefaultAsync(x => x.TokenId == tokenId &&
                                 x.Language == language);

            return translation != null ? _converter.Convert(translation) : default;
        }
    }
}
