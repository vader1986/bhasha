using Bhasha.Web.Domain;
using Bhasha.Web.Interfaces;

namespace Bhasha.Web.Services
{
    public class ChapterProvider : IChapterProvider
    {
        private readonly IRepository<Chapter> _chapterRepository;
        private readonly IRepository<Profile> _profileRepository;
        private readonly ITranslationProvider _translationProvider;
        private readonly IAsyncFactory<Page, Profile, DisplayedPage> _pageFactory;

        public ChapterProvider(
            IRepository<Chapter> chapterRepository,
            IRepository<Profile> profileRepository,
            ITranslationProvider translationProvider,
            IAsyncFactory<Page, Profile, DisplayedPage> pageFactory)
        {
            _chapterRepository = chapterRepository;
            _profileRepository = profileRepository;
            _translationProvider = translationProvider;
            _pageFactory = pageFactory;
        }

        public async Task<ChapterDescription[]> GetChapters(Guid profileId)
        {
            var profile = await _profileRepository.Get(profileId);
            if (profile == null) throw new ArgumentOutOfRangeException(nameof(profileId));

            var progress = profile.Progress;
            var chapters = await _chapterRepository.Find(x => x.RequiredLevel == progress.Level);

            var expressionIds = chapters.Select(x => x.NameId).Concat(chapters.Select(x => x.DescriptionId));
            var expressions = await _translationProvider.FindAll(profile.Native, expressionIds.ToArray());

            return chapters.Select(x =>
                new ChapterDescription(
                    x.Id,
                    expressions[x.NameId].Native,
                    expressions[x.DescriptionId].Native,
                    progress.CompletedChapters.Contains(x.Id))).ToArray();
        }

        public async Task<DisplayedChapter> GetChapter(Guid profileId, Guid chapterId)
        {
            var profile = await _profileRepository.Get(profileId);
            if (profile == null) throw new ArgumentOutOfRangeException(nameof(profileId));

            var chapter = await _chapterRepository.Get(chapterId);
            if (chapter == null) throw new ArgumentOutOfRangeException(nameof(chapterId));

            var pages = await Task.WhenAll(chapter.Pages.Select(
                async page => await _pageFactory.CreateAsync(page, profile)));

            var name = await _translationProvider.Find(chapter.NameId, profile.Native)
                ?? throw new InvalidOperationException($"Translation for {chapter.NameId} to {profile.Native} not found");

            var description = await _translationProvider.Find(chapter.DescriptionId, profile.Native)
                ?? throw new InvalidOperationException($"Translation for {chapter.DescriptionId} to {profile.Native} not found");

            return new DisplayedChapter(chapterId, name.Native, description.Native, pages, chapter.ResourceId);
        }
    }
}

