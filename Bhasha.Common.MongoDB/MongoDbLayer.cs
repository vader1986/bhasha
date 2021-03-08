using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Bhasha.Common.MongoDB
{
    public class MongoDbLayer : IDatabase
    {
        private readonly IMongoDb _db;

        public MongoDbLayer(IMongoDb db)
        {
            _db = db;
        }

        public async ValueTask<Chapter> CreateChapter(Chapter chapter)
        {
            var collection = _db.GetCollection<ChapterDto>(Names.Collections.Chapters);
            var dto = Converter.Convert(chapter);

            await collection.InsertOneAsync(dto);

            return new Chapter(
                dto.Id,
                chapter.Level,
                chapter.Name,
                chapter.Description,
                chapter.Pages,
                chapter.PictureId);
        }

        public async ValueTask<ChapterStats> CreateChapterStats(ChapterStats chapterStats)
        {
            var collection = _db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            var dto = Converter.Convert(chapterStats);

            await collection.InsertOneAsync(dto);

            return Converter.Convert(dto);
        }

        public async ValueTask<Profile> CreateProfile(Profile profile)
        {
            var collection = _db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            var dto = Converter.Convert(profile);

            await collection.InsertOneAsync(dto);

            return Converter.Convert(dto);
        }

        public async ValueTask<Tip> CreateTip(Tip tip)
        {
            var collection = _db.GetCollection<TipDto>(Names.Collections.Tips);
            var dto = Converter.Convert(tip);

            await collection.InsertOneAsync(dto);

            return Converter.Convert(dto);
        }

        public async ValueTask<User> CreateUser(User user)
        {
            var collection = _db.GetCollection<UserDto>(Names.Collections.Users);
            var dto = Converter.Convert(user);

            await collection.InsertOneAsync(dto);

            return Converter.Convert(dto);
        }

        public async ValueTask<int> DeleteChapter(Guid chapterId)
        {
            var collection = _db.GetCollection<ChapterDto>(Names.Collections.Chapters);
            var result = await collection.DeleteOneAsync(x => x.Id == chapterId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteChapterStats(Guid profileId)
        {
            var collection = _db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            var result = await collection.DeleteManyAsync(x => x.ProfileId == profileId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteProfile(Guid profileId)
        {
            var collection = _db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            var result = await collection.DeleteOneAsync(x => x.Id == profileId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteProfiles(Guid userId)
        {
            var collection = _db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            var result = await collection.DeleteManyAsync(x => x.UserId == userId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteTip(Guid tipId)
        {
            var collection = _db.GetCollection<TipDto>(Names.Collections.Tips);
            var result = await collection.DeleteOneAsync(x => x.Id == tipId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteTips(Guid chapterId, int pageIndex)
        {
            var collection = _db.GetCollection<TipDto>(Names.Collections.Tips);
            var result = await collection.DeleteManyAsync(
                x => x.ChapterId == chapterId &&
                     x.PageIndex == pageIndex);

            return (int)result.DeletedCount;
        }

        public async ValueTask<int> DeleteUser(Guid userId)
        {
            var collection = _db.GetCollection<UserDto>(Names.Collections.Users);
            var result = await collection.DeleteOneAsync(x => x.Id == userId);

            return (int)result.DeletedCount;
        }

        public async ValueTask<IEnumerable<Chapter>> GetChapters(int level)
        {
            var collection = _db.GetCollection<ChapterDto>(Names.Collections.Chapters);
            var chapters = await collection
                .AsQueryable()
                .Where(x => x.Level <= level)
                .ToListAsync();

            var tokenIds = chapters
                .SelectMany(c => c.Pages.Select(p => p.TokenId));

            var tokens = await _db
                .GetCollection<TokenDto>(Names.Collections.Tokens)
                .FindAsync(Builders<TokenDto>.Filter.In(x => x.Id, tokenIds));

            return chapters.Select(chapter =>
                Converter.Convert(
                    chapter,
                    tokens
                        .ToEnumerable()
                        .ToDictionary(x => x.Id, x => x)));
        }

        public async ValueTask<ChapterStats> GetChapterStats(Guid profileId, Guid chapterId)
        {
            var collection = _db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            var result = await collection
                .AsQueryable()
                .Where(x => x.ProfileId == profileId &&
                            x.ChapterId == chapterId)
                .ToListAsync();

            return result.Select(Converter.Convert).Single();
        }

        public async ValueTask<IEnumerable<Profile>> GetProfiles(Guid userId)
        {
            var collection = _db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            var result = await collection
                .AsQueryable()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            return result.Select(Converter.Convert);
        }

        public async ValueTask<IEnumerable<Tip>> GetTips(Guid chapterId, int pageIndex)
        {
            var collection = _db.GetCollection<TipDto>(Names.Collections.Tips);
            var result = await collection
                .AsQueryable()
                .Where(x => x.ChapterId == chapterId &&
                            x.PageIndex == pageIndex)
                .ToListAsync();

            return result.Select(Converter.Convert);
        }

        public async ValueTask<User> GetUser(Guid userId)
        {
            var collection = _db.GetCollection<UserDto>(Names.Collections.Users);
            var result = await collection
                .AsQueryable()
                .Where(x => x.Id == userId)
                .SingleAsync();

            return Converter.Convert(result);
        }

        public async ValueTask UpdateChapter(Chapter chapter)
        {
            var collection = _db.GetCollection<ChapterDto>(Names.Collections.Chapters);
            var dto = Converter.Convert(chapter);

            await collection.ReplaceOneAsync(x => x.Id == dto.Id, dto);
        }

        public async ValueTask UpdateChapterStats(ChapterStats chapterStats)
        {
            var collection = _db.GetCollection<ChapterStatsDto>(Names.Collections.Stats);
            var dto = Converter.Convert(chapterStats);

            await collection.ReplaceOneAsync(
                x => x.ChapterId == dto.ChapterId &&
                     x.ProfileId == dto.ProfileId, dto);
        }

        public async ValueTask UpdateProfile(Profile profile)
        {
            var collection = _db.GetCollection<ProfileDto>(Names.Collections.Profiles);
            var dto = Converter.Convert(profile);

            await collection.ReplaceOneAsync(x => x.Id == profile.Id, dto);
        }

        public async ValueTask UpdateTip(Tip tip)
        {
            var collection = _db.GetCollection<TipDto>(Names.Collections.Tips);
            var dto = Converter.Convert(tip);

            await collection.ReplaceOneAsync(x => x.Id == tip.Id, dto);
        }

        public async ValueTask UpdateUser(User user)
        {
            var collection = _db.GetCollection<UserDto>(Names.Collections.Users);
            var dto = Converter.Convert(user);

            await collection.ReplaceOneAsync(x => x.Id == user.Id, dto);
        }
    }
}
