using System;
using System.Linq;
using System.Threading.Tasks;
using Bhasha.Common.MongoDB.Dto;
using Bhasha.Common.Extensions;
using Bhasha.Common.MongoDB.Tests.Support;
using Mongo2Go;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NUnit.Framework;
using System.Text;

namespace Bhasha.Common.MongoDB.Tests
{
    [TestFixture]
    public class MongoDbLayerTests
    {
        private MongoDbRunner _runner;
        private MongoDb _db;
        private MongoDbLayer _layer;

        [SetUp]
        public void Before()
        {
            _runner = MongoDbRunner.Start();
            _db = MongoDb.Create(_runner.ConnectionString);
            _layer = new MongoDbLayer(_db);
        }

        [TearDown]
        public void After()
        {
            _runner.Dispose();
        }

        [Test]
        public async Task CreateChapter()
        {
            var dto = ChapterDtoBuilder.Build();
            var tokens = dto
                .Pages
                .Select(p => p.TokenId)
                .ToDictionary(x => x, x => TokenDtoBuilder.Build(x));
            dto.Id = default;

            var chapter = await _layer.CreateChapter(Converter.Convert(dto, tokens));

            var result = _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .AsQueryable()
                .Where(x => x.Id == chapter.Id)
                .Single();

            Assert.That(result.Id, Is.Not.SameAs(default(Guid)));
            Assert.That(result.Name, Is.EqualTo(dto.Name));
            Assert.That(result.Description, Is.EqualTo(dto.Description));
            Assert.That(result.Level, Is.EqualTo(dto.Level));
            Assert.That(result.PictureId, Is.EqualTo(dto.PictureId));
            Assert.That(result.Pages.Length, Is.EqualTo(dto.Pages.Length));
        }

        [Test]
        public async Task CreateChapterStats()
        {
            var dto = ChapterStatsDtoBuilder.Build();
            var stats = await _layer.CreateChapterStats(Converter.Convert(dto));

            var result = _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .AsQueryable()
                .Where(
                    x => x.ChapterId == stats.ChapterId &&
                         x.ProfileId == stats.ProfileId)
                .Single();

            Assert.That(result.ChapterId, Is.EqualTo(dto.ChapterId));
            Assert.That(result.ProfileId, Is.EqualTo(dto.ProfileId));
            Assert.That(result.Completed, Is.EqualTo(dto.Completed));
            Assert.That(result.Tips, Is.EqualTo(dto.Tips));
            Assert.That(result.Submits, Is.EqualTo(dto.Submits));
            Assert.That(result.Failures, Is.EqualTo(dto.Failures));
        }

        [Test]
        public async Task CreateProfile()
        {
            var dto = ProfileDtoBuilder.Build();
            var profile = await _layer.CreateProfile(Converter.Convert(dto));

            var result = _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .AsQueryable()
                .Where(x => x.Id == profile.Id)
                .Single();

            Assert.That(result.Id, Is.Not.SameAs(default(Guid)));
            Assert.That(result.UserId, Is.EqualTo(dto.UserId));
            Assert.That(result.Level, Is.EqualTo(dto.Level));
            Assert.That(result.From, Is.EqualTo(dto.From));
            Assert.That(result.To, Is.EqualTo(dto.To));
        }

        [Test]
        public async Task CreateTip()
        {
            var tip = await _layer.CreateTip(new Tip(default, Guid.NewGuid(), 3, "hello"));

            var result = _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .AsQueryable()
                .Where(x => x.Id == tip.Id)
                .Single();

            Assert.That(result.Id, Is.Not.SameAs(default(Guid)));
            Assert.That(result.ChapterId, Is.EqualTo(tip.ChapterId));
            Assert.That(result.PageIndex, Is.EqualTo(3));
            Assert.That(result.Text, Is.EqualTo("hello"));
        }

        [Test]
        public async Task CreateUser()
        {
            var user = await _layer.CreateUser(new User(default, "Hello", "asdf@bhasha.com"));

            var result = _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .AsQueryable()
                .Where(x => x.Id == user.Id)
                .Single();

            Assert.That(result.Id, Is.Not.SameAs(default(Guid)));
            Assert.That(result.UserName, Is.EqualTo("Hello"));
            Assert.That(result.Email, Is.EqualTo("asdf@bhasha.com"));
        }

        [Test]
        public async Task DeleteChapter()
        {
            var dto = ChapterDtoBuilder.Build();

            await _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteChapter(dto.Id);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .AsQueryable()
                .Any(x => x.Id == dto.Id);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteChapterStatsForProfile()
        {
            var dto = ChapterStatsDtoBuilder.Build();

            await _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteChapterStatsForProfile(dto.ProfileId);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .AsQueryable()
                .Any(
                    x => x.ChapterId == dto.ChapterId &&
                         x.ProfileId == dto.ProfileId);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteChapterStatsForChapter()
        {
            var dto = ChapterStatsDtoBuilder.Build();

            await _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteChapterStatsForChapter(dto.ChapterId);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .AsQueryable()
                .Any(
                    x => x.ChapterId == dto.ChapterId &&
                         x.ProfileId == dto.ProfileId);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteProfile()
        {
            var dto = ProfileDtoBuilder.Build();

            await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteProfile(dto.Id);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .AsQueryable()
                .Any(x => x.Id == dto.Id);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteProfiles()
        {
            var userId = Guid.NewGuid();

            await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .InsertManyAsync(new[] {
                    ProfileDtoBuilder.Build(userId),
                    ProfileDtoBuilder.Build(userId),
                    ProfileDtoBuilder.Build(userId),
                    ProfileDtoBuilder.Build(Guid.NewGuid()),
                });

            var deleted = await _layer.DeleteProfiles(userId);

            Assert.That(deleted, Is.EqualTo(3));

            var found = _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .AsQueryable()
                .Any(x => x.UserId == userId);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteTip()
        {
            var dto = new TipDto {
                Id = Guid.NewGuid(),
                ChapterId = Guid.NewGuid(),
                PageIndex = 3,
                Text = "hello"
            };

            await _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteTip(dto.Id);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .AsQueryable()
                .Any(x => x.Id == dto.Id);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteTips()
        {
            var chapterId = Guid.NewGuid();
            var pageIndex = 5;

            await _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .InsertManyAsync(new[] {
                    new TipDto { Id = Guid.NewGuid(), ChapterId = chapterId, PageIndex = pageIndex, Text = "1" },
                    new TipDto { Id = Guid.NewGuid(), ChapterId = chapterId, PageIndex = pageIndex, Text = "2" },
                    new TipDto { Id = Guid.NewGuid(), ChapterId = chapterId, PageIndex = pageIndex, Text = "3" },
                    new TipDto { Id = Guid.NewGuid(), ChapterId = chapterId, PageIndex = pageIndex + 1, Text = "4" }
                });

            var deleted = await _layer.DeleteTips(chapterId, pageIndex);

            Assert.That(deleted, Is.EqualTo(3));

            var found = _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .AsQueryable()
                .Any(x => x.ChapterId == chapterId &&
                          x.PageIndex == pageIndex);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task DeleteUser()
        {
            var dto = new UserDto {
                Id = Guid.NewGuid(),
                UserName = "Hello",
                Email = "hello@bhasha.com"
            };

            await _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .InsertOneAsync(dto);

            var deleted = await _layer.DeleteUser(dto.Id);

            Assert.That(deleted, Is.EqualTo(1));

            var found = _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .AsQueryable()
                .Any(x => x.Id == dto.Id);

            Assert.That(found, Is.False);
        }

        [Test]
        public async Task GetChapters()
        {
            var dtos = Enumerable
                .Range(1, 10)
                .Select(x => ChapterDtoBuilder.Build(x))
                .ToArray();

            await _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .InsertManyAsync(dtos);

            await _db
                .GetCollection<TokenDto>(Names.Collections.Tokens)
                .InsertManyAsync(dtos
                    .SelectMany(x => x.Pages)
                    .Select(x => TokenDtoBuilder.Build(x.TokenId)));

            var chapters = await _layer.GetChapters(5);

            foreach (var chapter in chapters)
            {
                Assert.That(chapter.Level <= 5);
            }
        }

        [Test]
        public async Task GetChapterStats()
        {
            var dto = ChapterStatsDtoBuilder.Build();

            await _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .InsertOneAsync(dto);

            var result = await _layer.GetChapterStats(dto.ProfileId, dto.ChapterId);

            Assert.That(result.Completed, Is.EqualTo(dto.Completed));
            Assert.That(result.Tips, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Tips)));
            Assert.That(result.Submits, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Submits)));
            Assert.That(result.Failures, Is.EqualTo(Encoding.UTF8.GetBytes(dto.Failures)));
        }

        [Test]
        public async Task GetProfiles()
        {
            var userId = Guid.NewGuid();

            var dtos = Enumerable
                .Range(1, 10)
                .Select(x => ProfileDtoBuilder.Build(x <= 5 ? userId : Guid.NewGuid()))
                .ToArray();

            await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .InsertManyAsync(dtos);

            var profiles = await _layer.GetProfiles(userId);

            foreach (var profile in profiles)
            {
                Assert.That(profile.UserId == userId);
            }
        }

        [Test]
        public async Task GetProfile()
        {
            var dtos = Enumerable
                .Range(1, 10)
                .Select(x => ProfileDtoBuilder.Build())
                .ToArray();

            await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .InsertManyAsync(dtos);

            var expectedProfile = dtos[0];
            var result = await _layer.GetProfile(expectedProfile.Id);


            Assert.That(result.Level == expectedProfile.Level);
            Assert.That(result.From == expectedProfile.From);
            Assert.That(result.To == expectedProfile.To);
            Assert.That(result.UserId == expectedProfile.UserId);
        }

        [Test]
        public async Task GetTips()
        {
            var chapterId = Guid.NewGuid();
            var pageIndex = 3;

            var dtos = Enumerable
                .Range(1, 10)
                .Select(x => new TipDto
                {
                    Id = Guid.NewGuid(),
                    ChapterId = x <= 5 ? chapterId : Guid.NewGuid(),
                    PageIndex = x <= 5 ? pageIndex : x,
                    Text = Rnd.Create.NextString()
                })
                .ToArray();

            await _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .InsertManyAsync(dtos);

            var tips = await _layer.GetTips(chapterId, pageIndex);

            foreach (var tip in tips)
            {
                Assert.That(tip.ChapterId == chapterId);
                Assert.That(tip.PageIndex == pageIndex);
            }
        }

        [Test]
        public async Task GetUser()
        {
            var userId = Guid.NewGuid();

            await _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .InsertOneAsync(new UserDto { Id = userId, Email = "x@y.com", UserName = "user" });

            var user = await _layer.GetUser(userId);

            Assert.That(user.Id == userId);
            Assert.That(user.Email == "x@y.com");
            Assert.That(user.UserName == "user");
        }

        [Test]
        public async Task UpdateChapter()
        {
            var dto = ChapterDtoBuilder.Build();

            await _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .InsertOneAsync(dto);

            var chapter = new Chapter(dto.Id, 1, "Updated Name", "Updated Description", new Page[0], default);

            await _layer.UpdateChapter(chapter);

            var chapters = await _db
                .GetCollection<ChapterDto>(Names.Collections.Chapters)
                .AsQueryable()
                .Where(x => x.Id == dto.Id)
                .ToListAsync();

            var result = chapters.ToArray();

            Assert.That(result.Length == 1);
            Assert.That(result[0].Level, Is.EqualTo(chapter.Level));
            Assert.That(result[0].Name, Is.EqualTo(chapter.Name));
            Assert.That(result[0].Description, Is.EqualTo(chapter.Description));
        }

        [Test]
        public async Task UpdateChapterStats()
        {
            var dto = ChapterStatsDtoBuilder.Build();

            await _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .InsertOneAsync(dto);

            var stats = new ChapterStats(
                dto.ProfileId,
                dto.ChapterId,
                true,
                Encoding.UTF8.GetBytes("123456"),
                Encoding.UTF8.GetBytes("654332"),
                Encoding.UTF8.GetBytes("324626"));

            await _layer.UpdateChapterStats(stats);

            var updatedStats = await _db
                .GetCollection<ChapterStatsDto>(Names.Collections.Stats)
                .AsQueryable()
                .SingleAsync(x => x.ProfileId == dto.ProfileId &&
                            x.ChapterId == dto.ChapterId);

            Assert.That(updatedStats.Completed, Is.EqualTo(stats.Completed));
            Assert.That(updatedStats.Tips, Is.EqualTo("123456"));
            Assert.That(updatedStats.Submits, Is.EqualTo("654332"));
            Assert.That(updatedStats.Failures, Is.EqualTo("324626"));
        }

        [Test]
        public async Task UpdateProfile()
        {
            var dto = ProfileDtoBuilder.Build();

            await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .InsertOneAsync(dto);

            await _layer.UpdateProfile(dto.Id, dto.Level + 1);

            var profile = await _db
                .GetCollection<ProfileDto>(Names.Collections.Profiles)
                .AsQueryable()
                .SingleAsync(x => x.Id == dto.Id);

            Assert.That(profile.Level == dto.Level + 1);
        }

        [Test]
        public async Task UpdateTip()
        {
            var dto = new TipDto {
                Id = Guid.NewGuid(),
                ChapterId = Guid.NewGuid(),
                PageIndex = 5,
                Text = "Hello World"
            };

            await _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .InsertOneAsync(dto);

            var updatedTip = new Tip(dto.Id, Guid.NewGuid(), 3, "New Text");

            await _layer.UpdateTip(updatedTip);

            var result = await _db
                .GetCollection<TipDto>(Names.Collections.Tips)
                .AsQueryable()
                .SingleAsync(x => x.Id == dto.Id);

            Assert.That(result.ChapterId == updatedTip.ChapterId);
            Assert.That(result.PageIndex == updatedTip.PageIndex);
            Assert.That(result.Text == updatedTip.Text);
        }

        [Test]
        public async Task UpdateUser()
        {
            var dto = new UserDto
            {
                Id = Guid.NewGuid(),
                UserName = "old_username",
                Email = "old@email.com"
            };

            await _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .InsertOneAsync(dto);

            var updatedUser = new User(dto.Id, "new_username", "new@email.com");

            await _layer.UpdateUser(updatedUser);

            var result = await _db
                .GetCollection<UserDto>(Names.Collections.Users)
                .AsQueryable()
                .SingleAsync(x => x.Id == dto.Id);

            Assert.That(result.UserName == updatedUser.UserName);
            Assert.That(result.Email == updatedUser.Email);
        }
    }
}
